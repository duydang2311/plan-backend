using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record Resource
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceId Id { get; init; }
    public string Name { get; init; } = null!;
    public UserId CreatorId { get; init; }
    public User Creator { get; init; } = null!;
    public string Rank { get; init; } = null!;
    public ResourceDocument? Document { get; init; }
    public ICollection<ResourceFile> Files { get; init; } = null!;
}
