using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record WorkspaceMemberInvited
{
    public required WorkspaceInvitationId WorkspaceInvitationId { get; init; }
    public required UserId MemberId { get; init; }
}
