namespace WebApp.Features.Tokens.Authenticate;

public sealed record class AuthenticateResult(
    string AccessToken,
    Guid RefreshToken,
    int AccessTokenMaxAge,
    int RefreshTokenMaxAge
);
