using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.WorkspaceMembers.Get;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public WorkspaceMemberId Id { get; init; }
        public UserId UserId { get; init; }
        public User User { get; init; } = null!;
        public RoleId RoleId { get; init; }
        public Role Role { get; init; } = null!;
        public WorkspaceId WorkspaceId { get; init; }
        public Workspace Workspace { get; init; } = null!;
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<WorkspaceMember> list);
}
