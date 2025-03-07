using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMemberInvitations.GetOne;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetOne;

public sealed record Request : Collective
{
    public ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.RequestingUserId))]
    public static partial GetProjectMemberInvitation ToCommand(this Request request);
}
