namespace WebApp.Api.V1.Teams.Create;

public sealed record Response
{
    public required Guid TeamId { get; init; }
    public required string Identifier { get; init; }
}
