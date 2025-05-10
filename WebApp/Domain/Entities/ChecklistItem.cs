using NodaTime;

namespace WebApp.Domain.Entities;

public record ChecklistItem
{
    public Instant CreatedTime { get; init; }
    public ChecklistItemId Id { get; init; }
    public IssueId ParentIssueId { get; init; }
    public Issue ParentIssue { get; init; } = null!;
    public ChecklistItemKind Kind { get; init; }
    public string? Content { get; init; }
    public IssueId? SubIssueId { get; init; }
    public Issue? SubIssue { get; init; }
}

public enum ChecklistItemKind
{
    Simple,
    SubIssue,
}
