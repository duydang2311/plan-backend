using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueAssignees.GetMany;

public sealed record GetIssueAssignees : Collective, ICommand<PaginatedList<IssueAssignee>>
{
    public required IssueId IssueId { get; init; }
    public string? Select { get; init; }
}
