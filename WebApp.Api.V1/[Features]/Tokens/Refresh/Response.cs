namespace WebApp.Api.V1.Tokens.Refresh;

public sealed record class Response(string AccessToken, int AccessTokenMaxAge);
