using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class UserGoogleAuth
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public string GoogleId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
