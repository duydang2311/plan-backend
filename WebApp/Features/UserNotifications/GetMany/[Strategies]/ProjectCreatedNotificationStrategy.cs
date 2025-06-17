using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class ProjectCreatedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.ProjectCreated;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (
            !string.IsNullOrEmpty(command.SelectProject)
            && notification.Notification?.Data?.RootElement.TryGetProperty("projectId", out var projIdElem) == true
            && Guid.TryParseExact(projIdElem.GetString(), "D", out var projGuid)
        )
        {
            context.ProjectIds.Add(new ProjectId { Value = projGuid });
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (context.HasLoaded<Project>())
        {
            return;
        }

        var ids = context.CollectIdContext.ProjectIds;
        var select = context.Command.SelectProject;

        if (!string.IsNullOrEmpty(select) && ids.Count > 0)
        {
            context.HydrateContext.ProjectsMap = await context
                .Db.Projects.Where(p => ids.Contains(p.Id))
                .Select(ExpressionHelper.Select<Project, Project>(select))
                .ToDictionaryAsync(p => p.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        context.MarkAsLoaded<Project>();
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectProject) || notification.Notification?.Data is null)
        {
            return notification;
        }

        if (
            notification.Notification.Data.RootElement.TryGetProperty("projectId", out var projIdElem)
            && Guid.TryParseExact(projIdElem.GetString(), "D", out var projGuid)
        )
        {
            var projectId = new ProjectId { Value = projGuid };
            if (context.ProjectsMap.TryGetValue(projectId, out var project))
            {
                var data = JsonSerializer.SerializeToDocument(project, jsonSerializerOptions);
                return notification with { Notification = notification.Notification with { Data = data } };
            }
        }
        return notification;
    }
}
