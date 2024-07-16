using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Teams.Create;

public sealed record CreateTeam : ICommand<OneOf<ValidationFailures, Team>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required UserId UserId { get; init; }
    public required string Name { get; init; }
    public required string Identifier { get; init; }
}
