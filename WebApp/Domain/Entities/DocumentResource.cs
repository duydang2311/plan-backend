namespace WebApp.Domain.Entities;

public sealed record DocumentResource : Resource
{
    public string Content { get; init; } = null!;
}
