using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record UserFriend
{
    public Instant CreatedTime { get; init; }
    public UserId UserId { get; init; } = UserId.Empty;
    public User User { get; init; } = null!;
    public UserId FriendId { get; init; } = UserId.Empty;
    public User Friend { get; init; } = null!;
}
