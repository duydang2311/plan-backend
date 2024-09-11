namespace WebApp.Domain.Entities;

public sealed record UserWorkspaceRole : UserRole
{
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
