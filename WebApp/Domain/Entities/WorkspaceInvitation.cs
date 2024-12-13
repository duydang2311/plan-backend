using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record WorkspaceInvitation
{
    public Instant CreatedTime { get; init; }
    public WorkspaceInvitationId Id { get; init; } = WorkspaceInvitationId.Empty;
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public Workspace Workspace { get; init; } = null!;
    public UserId UserId { get; init; } = UserId.Empty;
    public User User { get; init; } = null!;
}
