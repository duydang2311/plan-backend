using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record WorkspaceFieldDefinition
{
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public string Name { get; init; } = string.Empty;
    public FieldType Type { get; init; }
    public string? Description { get; init; }
}
