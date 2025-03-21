using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ChatMessage
{
    public Instant CreatedTime { get; init; }
    public ChatMessageId Id { get; init; }
    public ChatId ChatId { get; init; }
    public Chat Chat { get; init; } = null!;
    public UserId SenderId { get; init; }
    public User Sender { get; init; } = null!;
    public string Content { get; init; } = null!;
}
