using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMembers.GetPermissions;

namespace WebApp.Api.V1.ProjectMembers.GetPermissions.ByUser;

public sealed record Request
{
    public ProjectId ProjectId { get; init; }
    public UserId UserId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class RequestMapper
{
    [MapperIgnoreTarget(nameof(GetProjectMemberPermissions.Id))]
    public static partial GetProjectMemberPermissions ToCommand(this Request request);
}
