using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record IssueTimeline
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public Instant StartTime { get; init; }
    public Instant EndTime { get; init; }
}
