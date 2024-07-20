using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record Issue
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueId Id { get; init; }
    public UserId AuthorId { get; init; }
    public User Author { get; init; } = null!;
    public TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
    public long OrderNumber { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
}
