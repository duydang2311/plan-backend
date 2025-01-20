using NodaTime;

namespace WebApp.Domain.Entities;

public abstract record UserRole
{
    public Instant CreatedTime { get; init; }
    public UserRoleId UserRoleId { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public RoleId RoleId { get; init; }
    public Role Role { get; init; } = null!;
}
