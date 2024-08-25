namespace WebApp.Domain.Entities;

public sealed record WorkspaceFieldDefinition : FieldDefinition
{
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
}
