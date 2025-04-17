using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceInvitations.GetOne;

namespace WebApp.Api.V1.WorkspaceInvitations.GetOne;

public sealed record Request
{
    public WorkspaceInvitationId Id { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    public static partial GetWorkspaceInvitation ToCommand(this Request request);
}
