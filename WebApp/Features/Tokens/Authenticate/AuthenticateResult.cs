namespace WebApp.Features.Tokens.Authenticate;

public sealed record class AuthenticateResult(
    string AccessToken,
    string RefreshToken,
    int AccessTokenMaxAge,
    int RefreshTokenMaxAge
);
