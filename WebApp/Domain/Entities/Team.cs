using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class Team
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public WorkspaceId WorkspaceId { get; init; } = WorkspaceId.Empty;
    public Workspace Workspace { get; init; } = null!;
    public TeamId Id { get; init; } = TeamId.Empty;
    public string Name { get; init; } = string.Empty;
    public string Identifier { get; init; } = string.Empty;

    // Relationships
    public ICollection<User> Members { get; set; } = null!;
    public ICollection<TeamMember> TeamMembers { get; set; } = null!;
    public ICollection<TeamIssue> TeamIssues { get; set; } = null!;
    public ICollection<Issue> Issues { get; set; } = null!;
    public ICollection<Project> Projects { get; set; } = null!;
}
