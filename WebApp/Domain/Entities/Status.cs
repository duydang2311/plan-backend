namespace WebApp.Domain.Entities;

public sealed record Status
{
    public WorkspaceId? WorkspaceId { get; init; }
    public Workspace? Workspace { get; init; }
    public StatusId Id { get; init; }
    public int Order { get; init; }
    public string Value { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string? Description { get; init; }
}
