using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamInvitations.GetMany;

public sealed record GetTeamInvitations : Collective, ICommand<PaginatedList<TeamInvitation>>
{
    public required TeamId TeamId { get; init; }
    public string? Select { get; init; }
}
