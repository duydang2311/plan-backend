using NodaTime;
using WebApp.Common.Interfaces;
using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public sealed record Chat : ISoftDelete
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public ChatId Id { get; init; } = ChatId.Empty;
    public ChatType Type { get; init; }
    public string? Title { get; init; }
    public Instant? DeletedTime { get; init; }
    public UserId OwnerId { get; init; } = UserId.Empty;
    public User Owner { get; init; } = null!;

    public ICollection<ChatMember> ChatMembers { get; init; } = null!;
    public ICollection<User> Members { get; init; } = null!;
    public ICollection<ChatMessage> ChatMessages { get; init; } = null!;

    // Ignored
    public ChatMessage? LastChatMessage { get; init; }
}
