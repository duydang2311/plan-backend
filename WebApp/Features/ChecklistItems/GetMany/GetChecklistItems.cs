using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ChecklistItems.GetMany;

public sealed record GetChecklistItems : KeysetPagination<ChecklistItemId?>, ICommand<PaginatedList<ChecklistItem>>
{
    public required IssueId ParentIssueId { get; init; }
    public string? Select { get; init; }
}
