using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamMembers.GetMany;

public sealed record GetTeamMembers : Collective, ICommand<PaginatedList<TeamMember>>
{
    public required TeamId TeamId { get; init; }
    public string? Select { get; init; }
}
