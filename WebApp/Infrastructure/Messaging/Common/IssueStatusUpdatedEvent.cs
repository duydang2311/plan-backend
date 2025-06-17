using WebApp.Domain.Constants;

namespace WebApp.Infrastructure.Messaging.Common;

public sealed record IssueStatusUpdatedEvent(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    long OrderNumber,
    string Title,
    string ProjectIdentifier,
    string WorkspacePath,
    string OldStatusName,
    string NewStatusName
) { }
