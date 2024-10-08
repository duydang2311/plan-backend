using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.TeamMembers.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public TeamId TeamId { get; init; } = TeamId.Empty;
        public Team Team { get; init; } = null!;
        public UserId MemberId { get; init; } = UserId.Empty;
        public User Member { get; init; } = null!;
        public TeamRoleId RoleId { get; init; }
        public TeamRole Role { get; init; } = null!;
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<TeamMember> paginatedList);
}
