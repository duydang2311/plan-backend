using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record ProjectMemberInvitation
{
    public Instant CreatedTime { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; } = null!;
    public RoleId RoleId { get; init; }
    public Role Role { get; init; } = null!;
    public ProjectId ProjectId { get; init; }
    public Project Project { get; init; } = null!;
}
