namespace WebApp.Domain.Entities;

public sealed record DocumentResource : Resource
{
    public string Name { get; init; } = null!;
    public string Content { get; init; } = null!;
}
