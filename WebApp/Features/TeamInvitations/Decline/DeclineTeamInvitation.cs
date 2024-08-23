using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.TeamInvitations.Decline;

public sealed record DeclineTeamInvitation : ICommand<OneOf<ValidationFailures, Success>>
{
    public TeamInvitationId? TeamInvitationId { get; init; }
    public TeamId? TeamId { get; init; }
    public UserId? MemberId { get; init; }
}
