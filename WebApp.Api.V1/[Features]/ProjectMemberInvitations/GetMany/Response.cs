using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant? CreatedTime { get; init; }
        public ProjectMemberInvitationId? Id { get; init; }
        public UserId? UserId { get; init; }
        public UserDto? User { get; init; }
        public RoleId? RoleId { get; init; }
        public RoleDto? Role { get; init; } = null!;
    }

    public sealed record UserDto
    {
        public Instant? CreatedTime { get; init; }
        public Instant? UpdatedTime { get; init; }
        public UserId? Id { get; init; }
        public string? Email { get; init; }
        public UserProfile? Profile { get; init; } = null!;
    }

    public sealed record RoleDto
    {
        public RoleId? Id { get; init; }
        public string? Name { get; init; }
        public ICollection<RolePermission>? Permissions { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<ProjectMemberInvitation> list);

    public static partial Response.Item ToResponse(ProjectMemberInvitation invitation);

    public static Response.UserDto? ToNullableUser(this User user) => user?.ToNullableUserInternal();

    public static partial Response.UserDto? ToNullableUserInternal(this User? user);
}
