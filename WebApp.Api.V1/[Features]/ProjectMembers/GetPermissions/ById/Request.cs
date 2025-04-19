using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMembers.GetPermissions;

namespace WebApp.Api.V1.ProjectMembers.GetPermissions.ById;

public sealed record Request
{
    public ProjectMemberId? Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapperIgnoreTarget(nameof(GetProjectMemberPermissions.ProjectId))]
    [MapperIgnoreTarget(nameof(GetProjectMemberPermissions.UserId))]
    public static partial GetProjectMemberPermissions ToCommand(this Request request);
}
