using NodaTime;
using WebApp.Common.Models;

namespace WebApp.Domain.Entities;

public sealed record class UserProfile
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public UserId UserId { get; init; } = UserId.Empty;
    public User User { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string DisplayName { get; init; } = null!;
    public Asset Image { get; init; } = Asset.Empty;
    public string? Bio { get; init; } = null!;
    public ICollection<UserSocialLink>? SocialLinks { get; init; }
}
