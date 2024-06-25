using NodaTime;

namespace WebApp.SharedKernel.Models;

public sealed record class UserRefreshToken
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public UserId UserId { get; init; } = UserId.Empty;
    public User User { get; init; } = null!;
    public RefreshToken Token { get; init; } = RefreshToken.Empty;
}
