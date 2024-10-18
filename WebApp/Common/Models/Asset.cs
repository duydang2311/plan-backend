namespace WebApp.Common.Models;

public sealed record Asset
{
    public static readonly Asset Empty = new();

    public string? ResourceType { get; init; }
    public string? PublicId { get; init; }
    public string? Format { get; init; }
    public int? Version { get; init; }
}
