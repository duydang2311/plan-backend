using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record IssueAssignee
{
    public Instant CreatedTime { get; init; }
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
}
