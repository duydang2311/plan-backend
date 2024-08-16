using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamInvitations.Create;

public sealed record CreateTeamInvitation : ICommand<OneOf<ValidationFailures, TeamInvitation>>
{
    public required TeamId TeamId { get; init; }
    public required UserId MemberId { get; init; }
}
