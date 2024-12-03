using System.Security.Claims;
using FastEndpoints;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Users.SignProfileImageUpload;

namespace WebApp.Api.V1.Users.SignProfileImageUpload;

public sealed record Request
{
    [FromClaim(ClaimTypes.NameIdentifier)]
    public UserId UserId { get; init; }
}

[Mapper]
public static partial class RequestMapper
{
    public static partial SignProfileImageUploadCommand ToCommand(this Request request);
}
