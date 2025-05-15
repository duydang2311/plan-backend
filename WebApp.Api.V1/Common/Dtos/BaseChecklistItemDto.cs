using NodaTime;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseChecklistItemDto
{
    public Instant? CreatedTime { get; init; }
    public ChecklistItemId? Id { get; init; }
    public IssueId? ParentIssueId { get; init; }
    public BaseIssueDto? ParentIssue { get; init; }
    public ChecklistItemKind? Kind { get; init; }
    public string? Content { get; init; }
    public bool? Completed { get; init; }
    public IssueId? SubIssueId { get; init; }
    public BaseIssueDto? SubIssue { get; init; }
}
