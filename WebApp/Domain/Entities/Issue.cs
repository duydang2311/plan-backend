using NodaTime;
using WebApp.Common.Interfaces;

namespace WebApp.Domain.Entities;

public sealed record Issue : ISoftDelete
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
    public bool IsDeleted { get; init; }
    public Instant? DeletedTime { get; init; }
    public StatusId? StatusId { get; init; }
    public Status? Status { get; init; }
    public long OrderByStatus { get; init; }
    public ICollection<IssueComment> Comments { get; init; } = null!;
    public ICollection<IssueField> Fields { get; init; } = null!;
}
