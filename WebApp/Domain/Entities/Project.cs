using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class Project
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public ProjectId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public string? Description { get; init; }
    public ICollection<Status> Statuses { get; init; } = null!;
    public ICollection<Issue> Issues { get; init; } = null!;
    public ICollection<Team> Teams { get; init; } = null!;
}
