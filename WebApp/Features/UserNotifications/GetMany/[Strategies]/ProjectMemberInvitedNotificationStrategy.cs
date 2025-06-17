using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class ProjectMemberInvitedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.ProjectMemberInvited;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (
            !string.IsNullOrEmpty(command.SelectProjectMemberInvitation)
            && notification.Notification?.Data?.RootElement.TryGetProperty(
                "projectMemberInvitationId",
                out var inviteIdElem
            ) == true
            && inviteIdElem.TryGetInt64(out var inviteIdValue)
        )
        {
            context.ProjectMemberInvitationIds.Add(new ProjectMemberInvitationId { Value = inviteIdValue });
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (context.HasLoaded<ProjectMemberInvitation>())
        {
            return;
        }

        var ids = context.CollectIdContext.ProjectMemberInvitationIds;
        var select = context.Command.SelectProjectMemberInvitation;

        if (!string.IsNullOrEmpty(select) && ids.Count > 0)
        {
            context.HydrateContext.ProjectMemberInvitationsMap = await context
                .Db.ProjectMemberInvitations.Where(inv => ids.Contains(inv.Id))
                .Select(ExpressionHelper.Select<ProjectMemberInvitation, ProjectMemberInvitation>(select))
                .ToDictionaryAsync(inv => inv.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        context.MarkAsLoaded<ProjectMemberInvitation>();
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectProjectMemberInvitation) || notification.Notification?.Data is null)
        {
            return notification;
        }

        if (
            notification.Notification.Data.RootElement.TryGetProperty("projectMemberInvitationId", out var inviteIdElem)
            && inviteIdElem.TryGetInt64(out var inviteIdValue)
        )
        {
            var inviteId = new ProjectMemberInvitationId { Value = inviteIdValue };
            if (context.ProjectMemberInvitationsMap.TryGetValue(inviteId, out var invitation))
            {
                var data = JsonSerializer.SerializeToDocument(invitation, jsonSerializerOptions);
                return notification with { Notification = notification.Notification with { Data = data } };
            }
        }
        return notification;
    }
}
