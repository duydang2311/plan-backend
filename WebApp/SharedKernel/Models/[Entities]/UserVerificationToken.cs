namespace WebApp.SharedKernel.Models;

public sealed record class UserVerificationToken
{
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public Guid Token { get; init; }
}
