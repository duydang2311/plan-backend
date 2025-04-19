using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceMembers.GetPermissions;

namespace WebApp.Api.V1.WorkspaceMembers.GetPermissions.ById;

public sealed record Request
{
    public WorkspaceMemberId? Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapperIgnoreTarget(nameof(GetWorkspaceMemberPermissions.WorkspaceId))]
    [MapperIgnoreTarget(nameof(GetWorkspaceMemberPermissions.UserId))]
    public static partial GetWorkspaceMemberPermissions ToCommand(this Request request);
}
