namespace WebApp.Domain.Entities;

public sealed record WorkspaceStatus : Status
{
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public bool IsDefault { get; init; }
}
