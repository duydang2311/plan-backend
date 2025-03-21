using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ChatMember
{
    public Instant CreatedTime { get; init; }
    public ChatId ChatId { get; init; }
    public Chat Chat { get; init; } = null!;
    public UserId MemberId { get; init; }
    public User Member { get; init; } = null!;
    public ChatMessageId? LastReadMessageId { get; init; }
    public ChatMessage? LastReadMessage { get; init; }
}
