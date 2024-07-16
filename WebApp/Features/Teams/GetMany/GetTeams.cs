using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Teams.GetMany;

public sealed record GetTeams : Collective, ICommand<PaginatedList<Team>>
{
    public UserId? UserId { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}
