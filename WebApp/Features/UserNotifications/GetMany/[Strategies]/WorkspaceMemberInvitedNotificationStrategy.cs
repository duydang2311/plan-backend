using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class WorkspaceMemberInvitedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.WorkspaceMemberInvited;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (
            !string.IsNullOrEmpty(command.SelectWorkspaceInvitation)
            && notification.Notification?.Data?.RootElement.TryGetProperty("workspaceInvitationId", out var idElement)
                == true
            && idElement.TryGetInt64(out var idValue)
        )
        {
            context.WorkspaceInvitationIds.Add(new WorkspaceInvitationId { Value = idValue });
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (context.HasLoaded<WorkspaceInvitation>())
        {
            return;
        }

        var ids = context.CollectIdContext.WorkspaceInvitationIds;
        var select = context.Command.SelectWorkspaceInvitation;

        if (!string.IsNullOrEmpty(select) && ids.Count > 0)
        {
            context.HydrateContext.WorkspaceInvitationsMap = await context
                .Db.WorkspaceInvitations.Where(inv => ids.Contains(inv.Id))
                .Select(ExpressionHelper.Select<WorkspaceInvitation, WorkspaceInvitation>(select))
                .ToDictionaryAsync(inv => inv.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        context.MarkAsLoaded<WorkspaceInvitation>();
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectWorkspaceInvitation) || notification.Notification?.Data is null)
        {
            return notification;
        }

        if (
            notification.Notification.Data.RootElement.TryGetProperty("workspaceInvitationId", out var idElement)
            && idElement.TryGetInt64(out var idValue)
        )
        {
            var id = new WorkspaceInvitationId { Value = idValue };
            if (context.WorkspaceInvitationsMap.TryGetValue(id, out var invitation))
            {
                var data = JsonSerializer.SerializeToDocument(invitation, jsonSerializerOptions);
                return notification with { Notification = notification.Notification with { Data = data } };
            }
        }
        return notification;
    }
}
