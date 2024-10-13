using System.Security.Claims;
using FastEndpoints;
using FluentValidation;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Users.GetOne;

namespace WebApp.Api.V1.Users.GetOne.ByProfileName;

public sealed record Request
{
    public string ProfileName { get; init; } = null!;

    public string? Select { get; init; }

    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId RequestingUserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    [MapperIgnoreSource(nameof(Request.RequestingUserId))]
    [MapperIgnoreTarget(nameof(GetUser.UserId))]
    public static partial GetUser ToCommand(this Request request);
}
