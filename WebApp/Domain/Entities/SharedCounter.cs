namespace WebApp.Domain.Entities;

public sealed record SharedCounter
{
    public Guid Id { get; init; }
    public long Count { get; init; }
}
