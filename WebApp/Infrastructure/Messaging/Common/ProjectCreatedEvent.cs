using WebApp.Domain.Constants;

namespace WebApp.Infrastructure.Messaging.Common;

public sealed record ProjectCreatedEvent(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    string Identifier,
    string Name,
    string WorkspacePath
) { }
