using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;

namespace WebApp.Features.Teams.GetOne;

public sealed record GetTeam : ICommand<OneOf<None, Team>>
{
    public TeamId? TeamId { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Identifier { get; init; }

    public string? Select { get; init; }
}
