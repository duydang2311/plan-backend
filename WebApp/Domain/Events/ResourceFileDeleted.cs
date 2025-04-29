namespace WebApp.Domain.Events;

public sealed record ResourceFileDeleted
{
    public required string Key { get; init; }
}
