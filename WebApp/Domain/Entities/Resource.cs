using NodaTime;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public abstract record Resource
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ResourceId Id { get; init; }
    public ResourceType Type { get; init; }
    public UserId CreatorId { get; init; }
    public User Creator { get; init; } = null!;

    public WorkspaceResource? WorkspaceResource { get; init; }
    public ProjectResource? ProjectResource { get; init; }
}
