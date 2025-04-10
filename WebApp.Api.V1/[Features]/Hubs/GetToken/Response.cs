namespace WebApp.Api.V1.Hubs.GetToken;

public sealed record Response
{
    public required string AccessToken { get; init; }
}
