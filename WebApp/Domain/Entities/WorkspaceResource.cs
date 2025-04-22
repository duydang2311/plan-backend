namespace WebApp.Domain.Entities;

public record WorkspaceResource
{
    public ResourceId ResourceId { get; init; }
    public Resource Resource { get; init; } = null!;
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
