using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.IssueComments.GetMany;

public sealed record GetIssueComments : Collective, ICommand<PaginatedList<IssueComment>>
{
    public IssueId? IssueId { get; init; }
    public string? Select { get; init; }
}
