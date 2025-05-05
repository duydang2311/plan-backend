using WebApp.Domain.Constants;

namespace WebApp.Infrastructure.Messaging.Common;

public sealed record ProjectMemberInvitedEvent(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    string ProjectMemberInvitationId,
    string ProjectIdentifier,
    string ProjectName
) { }
