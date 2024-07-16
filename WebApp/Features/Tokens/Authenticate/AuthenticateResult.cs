using WebApp.Domain.Entities;

namespace WebApp.Features.Tokens.Authenticate;

public sealed record class AuthenticateResult(
    string AccessToken,
    RefreshToken RefreshToken,
    int AccessTokenMaxAge,
    int RefreshTokenMaxAge
);
