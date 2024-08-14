using NodaTime;

namespace WebApp.Api.V1.Workspaces.Get;

public sealed record Response
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
}
