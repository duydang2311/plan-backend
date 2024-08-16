using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.TeamInvitations.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public TeamId TeamId { get; init; } = TeamId.Empty;
        public Team Team { get; init; } = null!;
        public UserId UserId { get; init; } = UserId.Empty;
        public User User { get; init; } = null!;
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<TeamInvitation> response);
}
