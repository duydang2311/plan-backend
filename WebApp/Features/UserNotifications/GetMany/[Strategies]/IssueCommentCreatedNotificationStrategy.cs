using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.GetMany;

public sealed class IssueCommentCreatedNotificationStrategy : INotificationProcessingStrategy
{
    public bool CanProcess(NotificationType type)
    {
        return type == NotificationType.IssueCommentCreated;
    }

    public void CollectIds(
        UserNotification notification,
        GetUserNotifications command,
        NotificationCollectIdContext context
    )
    {
        if (
            !string.IsNullOrEmpty(command.SelectComment)
            && notification.Notification?.Data?.RootElement.TryGetProperty("issueAuditId", out var auditIdElem) == true
            && auditIdElem.TryGetInt64(out var auditId)
        )
        {
            context.IssueAuditIds.Add(auditId);
        }
    }

    public async Task LoadDataAsync(NotificationLoadContext context, CancellationToken cancellationToken)
    {
        if (context.HasLoaded<IssueAudit>())
        {
            return;
        }

        var ids = context.CollectIdContext.IssueAuditIds;
        var select = context.Command.SelectComment;

        if (!string.IsNullOrEmpty(select) && ids.Count > 0)
        {
            context.HydrateContext.AuditsMap = await context
                .Db.IssueAudits.Where(a => ids.Contains(a.Id))
                .Select(ExpressionHelper.Select<IssueAudit, IssueAudit>(select))
                .ToDictionaryAsync(a => a.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        context.MarkAsLoaded<IssueAudit>();
    }

    public UserNotification HydrateData(
        UserNotification notification,
        GetUserNotifications command,
        NotificationHydrateContext context,
        JsonSerializerOptions jsonSerializerOptions
    )
    {
        if (string.IsNullOrEmpty(command.SelectComment) || notification.Notification?.Data is null)
        {
            return notification;
        }

        if (
            notification.Notification.Data.RootElement.TryGetProperty("issueAuditId", out var auditIdElem)
            && auditIdElem.TryGetInt64(out var auditId)
        )
        {
            if (context.AuditsMap.TryGetValue(auditId, out var audit))
            {
                var data = JsonSerializer.SerializeToDocument(audit, jsonSerializerOptions);
                return notification with { Notification = notification.Notification with { Data = data } };
            }
        }
        return notification;
    }
}
