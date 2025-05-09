using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record IssueLink
{
    public Instant CreatedTime { get; init; }
    public IssueLinkId Id { get; init; }
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public IssueId SubIssueId { get; init; }
    public Issue SubIssue { get; init; } = null!;
}
