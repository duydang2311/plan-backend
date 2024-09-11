namespace WebApp.Domain.Entities;

public sealed record WorkspaceMember : UserRole
{
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
