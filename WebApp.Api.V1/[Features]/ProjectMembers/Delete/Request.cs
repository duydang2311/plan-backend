using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectMembers.Delete;

namespace WebApp.Api.V1.ProjectMembers.Delete;

public sealed record Request
{
    public ProjectMemberId Id { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class RequestMapper
{
    public static partial DeleteProjectMember ToCommand(this Request request);
}
