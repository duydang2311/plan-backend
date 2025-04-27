namespace WebApp.Api.V1.WorkspaceResources.CreateToken;

public sealed record Response
{
    public required string AccessToken { get; init; }
}
