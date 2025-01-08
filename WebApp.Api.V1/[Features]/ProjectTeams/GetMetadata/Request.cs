using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.ProjectTeams.GetMetadata;

namespace WebApp.Api.V1.ProjectTeams.GetMetadata;

public sealed record Request
{
    public ProjectId? ProjectId { get; init; }
    public bool IncludeTotalCount { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.UserId))]
    public static partial GetProjectTeamMetadata ToCommand(this Request request);
}
