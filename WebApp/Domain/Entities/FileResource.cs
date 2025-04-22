namespace WebApp.Domain.Entities;

public sealed record FileResource : Resource
{
    public string Key { get; init; } = null!;
}
