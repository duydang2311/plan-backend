using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Users.GetOne;

namespace WebApp.Api.V1.Users.GetOne.ById;

public sealed record Request
{
    public UserId UserId { get; init; }

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.RequestingUserId))]
    [MapperIgnoreTarget(nameof(GetUser.ProfileName))]
    public static partial GetUser ToCommand(this Request request);
}
