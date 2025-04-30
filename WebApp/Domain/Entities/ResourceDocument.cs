using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ResourceDocument
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceId ResourceId { get; init; }
    public Resource Resource { get; init; } = null!;
    public string Content { get; init; } = null!;
    public string? PreviewContent { get; init; }
}
