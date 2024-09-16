using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Projects.GetOne;

namespace WebApp.Api.V1.Projects.GetOne.ByIdentifier;

public sealed record Request
{
    public string Identifier { get; init; } = null!;
    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial GetProject ToCommand(this Request request);
}
