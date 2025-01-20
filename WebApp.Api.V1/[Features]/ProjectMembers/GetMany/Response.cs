using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectMembers.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant? CreatedTime { get; init; }
        public UserRoleId? UserRoleId { get; init; }
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

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<ProjectMember> list);

    [MapperIgnoreSource(nameof(ProjectMember.ProjectId))]
    [MapperIgnoreSource(nameof(ProjectMember.Project))]
    public static partial Response.Item ToResponse(ProjectMember list);

    public static Response.UserDto? ToNullableUser(this User user) => user?.ToNullableUserInternal();

    [MapperIgnoreSource(nameof(User.Salt))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.IsVerified))]
    [MapperIgnoreSource(nameof(User.Teams))]
    [MapperIgnoreSource(nameof(User.WorkspaceMembers))]
    [MapperIgnoreSource(nameof(User.Workspaces))]
    [MapperIgnoreSource(nameof(User.GoogleAuth))]
    [MapperIgnoreSource(nameof(User.Issues))]
    [MapperIgnoreSource(nameof(User.Roles))]
    public static partial Response.UserDto? ToNullableUserInternal(this User? user);
}
