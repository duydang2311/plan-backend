using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class IssueCreatedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.IssueCreated;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (
            !string.IsNullOrEmpty(command.SelectIssue)
            && notification.Notification?.Data?.RootElement.TryGetProperty("issueId", out var issueIdElem) == true
            && Guid.TryParseExact(issueIdElem.GetString(), "D", out var issueGuid)
        )
        {
            context.IssueIds.Add(new IssueId { Value = issueGuid });
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (context.HasLoaded<Issue>())
        {
            return;
        }

        var ids = context.CollectIdContext.IssueIds;
        var select = context.Command.SelectIssue;

        if (!string.IsNullOrEmpty(select) && ids.Count > 0)
        {
            context.HydrateContext.IssuesMap = await context
                .Db.Issues.Where(i => ids.Contains(i.Id))
                .Select(ExpressionHelper.Select<Issue, Issue>(select))
                .ToDictionaryAsync(i => i.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        context.MarkAsLoaded<Issue>();
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectIssue) || notification.Notification?.Data is null)
        {
            return notification;
        }

        if (
            notification.Notification.Data.RootElement.TryGetProperty("issueId", out var issueIdElem)
            && Guid.TryParseExact(issueIdElem.GetString(), "D", out var issueGuid)
        )
        {
            var issueId = new IssueId { Value = issueGuid };
            if (context.IssuesMap.TryGetValue(issueId, out var issue))
            {
                var data = JsonSerializer.SerializeToDocument(issue, jsonSerializerOptions);
                return notification with { Notification = notification.Notification with { Data = data } };
            }
        }
        return notification;
    }
}
