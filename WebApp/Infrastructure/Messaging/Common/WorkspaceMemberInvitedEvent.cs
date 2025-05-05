using WebApp.Domain.Constants;

namespace WebApp.Infrastructure.Messaging.Common;

public sealed record WorkspaceMemberInvitedEvent(
    string UserId,
    long UserNotificationId,
    NotificationType Type,
    string WorkspaceInvitationId,
    string WorkspacePath,
    string WorkspaceName
) { }
