using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record WorkspaceMember
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceMemberId Id { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public RoleId RoleId { get; init; }
    public Role Role { get; init; } = null!;
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
