using NodaTime;

namespace WebApp.SharedKernel.Models;

public sealed record class User
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public UserId Id { get; init; } = UserId.Empty;
    public string Email { get; init; } = string.Empty;
    public byte[] Salt { get; init; } = [];
    public byte[] PasswordHash { get; init; } = [];
    public bool IsVerified { get; init; }
}
