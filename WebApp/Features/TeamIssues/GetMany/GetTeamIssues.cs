using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamIssues.GetMany;

public sealed record GetTeamIssues : Collective, ICommand<PaginatedList<TeamIssue>>
{
    public TeamId? TeamId { get; init; }
    public IssueId? IssueId { get; init; }
    public string? Select { get; init; }
}
