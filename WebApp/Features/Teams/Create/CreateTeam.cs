using FastEndpoints;
using OneOf;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Teams.Create;

public sealed record CreateTeam : ICommand<OneOf<ValidationFailures, Team>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required UserId UserId { get; init; }
    public required string Name { get; init; }
    public required string Identifier { get; init; }
}
