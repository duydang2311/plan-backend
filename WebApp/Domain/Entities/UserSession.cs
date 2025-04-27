using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class UserSession
{
    public Instant CreatedTime { get; init; }
    public SessionId SessionId { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
}
