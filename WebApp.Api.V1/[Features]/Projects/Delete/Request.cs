using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Projects.Delete;

namespace WebApp.Api.V1.Projects.Delete;

public sealed record Request
{
    public ProjectId ProjectId { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.UserId))]
    public static partial DeleteProject ToCommand(this Request request);
}
