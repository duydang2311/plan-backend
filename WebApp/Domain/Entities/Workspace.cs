using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class Workspace
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId Id { get; init; } = WorkspaceId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
}
