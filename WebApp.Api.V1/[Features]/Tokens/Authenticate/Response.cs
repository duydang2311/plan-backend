using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.Tokens.Authenticate;

namespace WebApp.Api.V1.Tokens.Authenticate;

public sealed record class Response(
    string AccessToken,
    RefreshToken RefreshToken,
    int AccessTokenMaxAge,
    int RefreshTokenMaxAge,
    SessionToken SessionId,
    int SessionMaxAge
);

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this AuthenticateResult result);
}
