using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record TeamInvitation
{
    public Instant CreatedTime { get; init; }
    public TeamId TeamId { get; init; } = TeamId.Empty;
    public Team Team { get; init; } = null!;
    public UserId UserId { get; init; } = UserId.Empty;
    public User User { get; init; } = null!;
}
