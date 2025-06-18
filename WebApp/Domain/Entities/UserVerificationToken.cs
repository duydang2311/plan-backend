namespace WebApp.Domain.Entities;

public sealed record UserVerificationToken
{
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public Guid Token { get; init; }
}
