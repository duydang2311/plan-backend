using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceInvitations.Delete;

namespace WebApp.Api.V1.WorkspaceInvitations.Delete;

public sealed record Request
{
    public WorkspaceInvitationId Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial DeleteWorkspaceInvitation ToCommand(this Request request);
}
