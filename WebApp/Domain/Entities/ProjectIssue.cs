using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ProjectIssue
{
    public Instant CreatedTime { get; init; }
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public string Rank { get; init; } = string.Empty;
}
