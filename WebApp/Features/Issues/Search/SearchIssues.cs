using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Search;

public sealed record SearchIssues : Collective, ICommand<PaginatedList<Issue>>
{
    public required ProjectId ProjectId { get; init; }
    public required string Query { get; init; }
    public string? Select { get; init; }
    public IReadOnlyCollection<IssueId>? ExcludeIssueIds { get; init; }
    public IssueId? ExcludeChecklistItemParentIssueId { get; init; }
    public float? Threshold { get; init; }
}
