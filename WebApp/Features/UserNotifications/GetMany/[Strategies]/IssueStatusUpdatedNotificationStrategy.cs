using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class IssueStatusUpdatedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.IssueStatusUpdated;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (string.IsNullOrEmpty(command.SelectStatus) || notification.Notification?.Data is null)
        {
            return;
        }

        var root = notification.Notification.Data.RootElement;

        if (root.TryGetProperty("issueId", out var issueIdElement) && issueIdElement.TryGetGuid(out var issueIdValue))
        {
            context.IssueIds.Add(new IssueId { Value = issueIdValue });
        }
        if (
            root.TryGetProperty("oldStatusId", out var statusIdElement)
            && statusIdElement.TryGetInt64(out var statusIdValue)
        )
        {
            context.StatusIds.Add(new StatusId { Value = statusIdValue });
        }
        if (root.TryGetProperty("newStatusId", out statusIdElement) && statusIdElement.TryGetInt64(out statusIdValue))
        {
            context.StatusIds.Add(new StatusId { Value = statusIdValue });
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (!context.HasLoaded<Issue>())
        {
            var ids = context.CollectIdContext.IssueIds;
            var select = context.Command.SelectIssue ?? "Id";
            if (ids.Count > 0)
            {
                context.HydrateContext.IssuesMap = await context
                    .Db.Issues.Where(i => ids.Contains(i.Id))
                    .Select(ExpressionHelper.Select<Issue, Issue>(select))
                    .ToDictionaryAsync(i => i.Id, cancellationToken)
                    .ConfigureAwait(false);
            }
            context.MarkAsLoaded<Issue>();
        }

        if (!context.HasLoaded<WorkspaceStatus>())
        {
            var ids = context.CollectIdContext.StatusIds;
            var select = context.Command.SelectStatus;
            if (!string.IsNullOrEmpty(select) && ids.Count > 0)
            {
                context.HydrateContext.StatusesMap = await context
                    .Db.WorkspaceStatuses.Where(s => ids.Contains(s.Id))
                    .Select(ExpressionHelper.Select<WorkspaceStatus, WorkspaceStatus>(select))
                    .ToDictionaryAsync(s => s.Id, cancellationToken)
                    .ConfigureAwait(false);
            }
            context.MarkAsLoaded<WorkspaceStatus>();
        }
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectStatus) || notification.Notification?.Data is null)
        {
            return notification;
        }

        var root = notification.Notification.Data.RootElement;
        var jsonObject = new JsonObject();

        if (root.TryGetProperty("issueId", out var issueIdElement) && issueIdElement.TryGetGuid(out var issueIdValue))
        {
            var issueId = new IssueId { Value = issueIdValue };
            if (context.IssuesMap.TryGetValue(issueId, out var issue))
            {
                jsonObject["issue"] = JsonSerializer.SerializeToNode(issue, jsonSerializerOptions);
            }
        }
        if (root.TryGetProperty("oldStatusId", out var idElement) && idElement.TryGetInt64(out var idValue))
        {
            var id = new StatusId { Value = idValue };
            if (context.StatusesMap.TryGetValue(id, out var status))
            {
                jsonObject["oldStatus"] = JsonSerializer.SerializeToNode(status, jsonSerializerOptions);
            }
        }
        if (root.TryGetProperty("newStatusId", out idElement) && idElement.TryGetInt64(out idValue))
        {
            var id = new StatusId { Value = idValue };
            if (context.StatusesMap.TryGetValue(id, out var status))
            {
                jsonObject["newStatus"] = JsonSerializer.SerializeToNode(status, jsonSerializerOptions);
            }
        }

        if (jsonObject.Count > 0)
        {
            var data = JsonSerializer.SerializeToDocument(jsonObject, jsonSerializerOptions);
            return notification with { Notification = notification.Notification with { Data = data } };
        }

        return notification;
    }
}
