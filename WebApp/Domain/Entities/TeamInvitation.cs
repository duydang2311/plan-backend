using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record TeamInvitation
{
    public Instant CreatedTime { get; init; }
    public TeamInvitationId Id { get; init; } = TeamInvitationId.Empty;
    public TeamId TeamId { get; init; } = TeamId.Empty;
    public Team Team { get; init; } = null!;
    public UserId MemberId { get; init; } = UserId.Empty;
    public User Member { get; init; } = null!;
}
