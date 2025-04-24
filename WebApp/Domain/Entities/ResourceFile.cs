using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ResourceFile
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceFileId Id { get; init; }
    public ResourceId ResourceId { get; init; }
    public Resource Resource { get; init; } = null!;
    public string Key { get; init; } = null!;
    public string OriginalName { get; init; } = null!;
}
