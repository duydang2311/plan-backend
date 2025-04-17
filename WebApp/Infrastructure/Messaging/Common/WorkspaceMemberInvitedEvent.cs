using WebApp.Domain.Constants;

namespace WebApp.Infrastructure.Messaging.Common;

public sealed record WorkspaceMemberInvitedEvent(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    long WorkspaceInvitationId,
    string WorkspacePath,
    string WorkspaceName
) { }
