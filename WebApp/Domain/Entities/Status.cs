namespace WebApp.Domain.Entities;

public sealed record Status
{
    public StatusId Id { get; init; }
    public int Rank { get; init; }
    public string Value { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string? Description { get; init; }
}
