using NodaTime;
using NpgsqlTypes;
using WebApp.Common.Interfaces;

namespace WebApp.Domain.Entities;

public sealed record class Project : ISoftDelete
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; }
    public Workspace Workspace { get; init; } = null!;
    public ProjectId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Instant? DeletedTime { get; init; }
    public NpgsqlTsVector SearchVector { get; init; } = null!;

    // public ICollection<Status> Statuses { get; init; } = null!;
    public ICollection<Issue> Issues { get; init; } = null!;
    public ICollection<Team> Teams { get; init; } = null!;
    public ICollection<ProjectMember> Members { get; init; } = null!;
    public ICollection<Milestone> Milestones { get; init; } = null!;
    public ICollection<MilestoneStatus> MilestoneStatuses { get; init; } = null!;
}
