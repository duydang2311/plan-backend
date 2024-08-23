using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.TeamInvitations.Accept;

namespace WebApp.Api.V1.TeamInvitations.Accept;

public sealed record Request
{
    public TeamInvitationId TeamInvitationId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial AcceptTeamInvitation ToCommand(this Request request);
}
