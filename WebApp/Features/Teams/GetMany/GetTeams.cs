using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Teams.GetMany;

public sealed record GetTeams : Collective, ICommand<PaginatedList<Team>>
{
    public UserId? UserId { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}
