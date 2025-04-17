using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record WorkspaceMemberInvitedUserNotified : UserNotified
{
    public required WorkspaceInvitationId WorkspaceInvitationId { get; init; }
    public required string WorkspacePath { get; init; }
    public required string WorkspaceName { get; init; }
}
