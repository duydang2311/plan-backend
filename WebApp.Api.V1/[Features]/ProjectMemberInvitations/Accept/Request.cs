using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMemberInvitations.Accept;

namespace WebApp.Api.V1.ProjectMemberInvitations.Accept;

public sealed record Request
{
    public ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial AcceptProjectMemberInvitation ToCommand(this Request request);
}
