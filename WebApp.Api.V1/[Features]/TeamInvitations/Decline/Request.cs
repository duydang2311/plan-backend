using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.TeamInvitations.Decline;

namespace WebApp.Api.V1.TeamInvitations.Decline;

public sealed record Request
{
    public TeamInvitationId TeamInvitationId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial DeclineTeamInvitation ToCommand(this Request request);
}
