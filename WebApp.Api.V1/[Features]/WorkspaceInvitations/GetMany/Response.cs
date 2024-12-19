using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.WorkspaceInvitations.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant? CreatedTime { get; init; }
        public WorkspaceInvitationId? Id { get; init; }
        public WorkspaceId? WorkspaceId { get; init; }
        public Workspace? Workspace { get; init; }
        public UserId? UserId { get; init; }
        public User? User { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<WorkspaceInvitation> list);
}
