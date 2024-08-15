using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record TeamMember
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public TeamId TeamId { get; init; } = TeamId.Empty;
    public Team Team { get; init; } = null!;
    public UserId MemberId { get; init; } = UserId.Empty;
    public User Member { get; init; } = null!;
    public TeamRoleId RoleId { get; init; }
    public TeamRole Role { get; init; } = null!;
    public TeamInvitation? PendingInvitation { get; init; }
    public bool IsInvitationPending => PendingInvitation is not null;
}
