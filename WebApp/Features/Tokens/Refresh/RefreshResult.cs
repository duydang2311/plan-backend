namespace WebApp.Features.Tokens.Refresh;

public sealed record class RefreshResult(string AccessToken, int AccessTokenMaxAge);
