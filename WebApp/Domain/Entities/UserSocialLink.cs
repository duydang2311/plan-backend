namespace WebApp.Domain.Entities;

public sealed record UserSocialLink
{
    public long Id { get; init; }
    public UserId UserId { get; init; }
    public UserProfile Profile { get; init; } = null!;
    public string Url { get; init; } = string.Empty;
}
