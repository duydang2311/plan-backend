using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record IssueComment
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueCommentId Id { get; init; }
    public UserId AuthorId { get; init; }
    public User Author { get; init; } = null!;
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public string Content { get; init; } = string.Empty;
}
