using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceMembers.GetPermissions;

namespace WebApp.Api.V1.WorkspaceMembers.GetPermissions.ByUser;

public sealed record Request
{
    public WorkspaceId WorkspaceId { get; init; }
    public UserId UserId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapperIgnoreTarget(nameof(GetWorkspaceMemberPermissions.Id))]
    public static partial GetWorkspaceMemberPermissions ToCommand(this Request request);
}
