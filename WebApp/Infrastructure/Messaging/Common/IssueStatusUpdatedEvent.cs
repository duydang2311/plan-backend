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
    byte? OldStatusCategory,
    string? OldStatusColor,
    string? OldStatusValue,
    byte? NewStatusCategory,
    string? NewStatusColor,
    string? NewStatusValue
) { }
