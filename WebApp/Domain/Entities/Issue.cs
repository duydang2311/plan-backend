using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record Issue
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueId Id { get; init; }
    public TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
