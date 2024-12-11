using NodaTime;
using WebApp.Common.Interfaces;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record Issue : ISoftDelete
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueId Id { get; init; }
    public UserId AuthorId { get; init; }
    public User Author { get; init; } = null!;
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public long OrderNumber { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public bool IsDeleted { get; init; }
    public Instant? DeletedTime { get; init; }
    public StatusId? StatusId { get; init; }
    public WorkspaceStatus? Status { get; init; }
    public string StatusRank { get; init; } = null!;
    public IssuePriority Priority { get; init; }
    public ICollection<IssueComment> Comments { get; init; } = null!;
    public ICollection<IssueField> Fields { get; init; } = null!;
    public ICollection<TeamIssue> TeamIssues { get; init; } = null!;
    public ICollection<Team> Teams { get; init; } = null!;
    public ICollection<IssueAssignee> IssueAssignees { get; init; } = null!;
    public ICollection<User> Assignees { get; init; } = null!;
}
