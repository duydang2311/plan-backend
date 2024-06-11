namespace WebApp.Api.V1.Tokens.Authenticate;

public sealed record class Response(
    string AccessToken,
    string RefreshToken,
    int AccessTokenMaxAge,
    int RefreshTokenMaxAge);
