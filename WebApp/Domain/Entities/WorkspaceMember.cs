namespace WebApp.Domain.Entities;

public sealed record WorkspaceMember
{
    public WorkspaceMemberId Id { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public RoleId RoleId { get; init; }
    public Role Role { get; init; } = null!;
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
