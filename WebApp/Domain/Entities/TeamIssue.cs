using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record TeamIssue
{
    public Instant CreatedTime { get; init; }
    public TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public string Rank { get; init; } = string.Empty;
}
