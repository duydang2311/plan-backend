using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.GetMany;

public sealed record GetIssues : Collective, ICommand<PaginatedList<Issue>>
{
    public TeamId? TeamId { get; init; }
    public string? Select { get; init; }
    public StatusId? StatusId { get; init; }
    public bool? NullStatusId { get; init; }
}
