namespace WebApp.Api.V1.Hubs.CreateToken;

public sealed record Response
{
    public required string AccessToken { get; init; }
}
