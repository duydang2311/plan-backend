using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMemberInvitations.Delete;

namespace WebApp.Api.V1.ProjectMemberInvitations.Delete;

public sealed record Request
{
    public ProjectMemberInvitationId Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class RequestMapper
{
    public static partial DeleteProjectMemberInvitation ToCommand(this Request request);
}
