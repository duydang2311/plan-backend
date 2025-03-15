using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record UserFriendRequest
{
    public Instant CreatedTime { get; init; }
    public UserId SenderId { get; init; } = UserId.Empty;
    public User Sender { get; init; } = null!;
    public UserId ReceiverId { get; init; } = UserId.Empty;
    public User Receiver { get; init; } = null!;
}
