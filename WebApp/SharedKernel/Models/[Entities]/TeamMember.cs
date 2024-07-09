using NodaTime;

namespace WebApp.SharedKernel.Models;

public sealed record TeamMember
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public TeamId TeamId { get; init; } = TeamId.Empty;
    public Team Team { get; init; } = null!;
    public UserId MemberId { get; init; } = UserId.Empty;
    public User Member { get; init; } = null!;
}
