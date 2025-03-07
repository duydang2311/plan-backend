using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetOne;

public sealed record Response
{
    public Instant? CreatedTime { get; init; }
    public ProjectMemberInvitationId? Id { get; init; }
    public UserId? UserId { get; init; }
    public UserDto? User { get; init; }
    public RoleId? RoleId { get; init; }
    public RoleDto? Role { get; init; }
    public ProjectId? ProjectId { get; init; }
    public ProjectDto? Project { get; init; }

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

    public sealed record ProjectDto
    {
        public Instant? CreatedTime { get; init; }
        public Instant? UpdatedTime { get; init; }
        public ProjectId? Id { get; init; }
        public string? Name { get; init; }
        public string? Identifier { get; init; }
        public string? Description { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this ProjectMemberInvitation list);

    public static Response.UserDto? ToNullableUser(this User user) => user?.ToNullableUserInternal();

    public static partial Response.UserDto? ToNullableUserInternal(this User? user);

    public static Response.RoleDto? ToNullableRole(this Role role) => role?.ToNullableRoleInternal();

    public static partial Response.RoleDto? ToNullableRoleInternal(this Role? role);

    public static Response.ProjectDto? ToNullableProject(this Project project) => project?.ToNullableProjectInternal();

    public static partial Response.ProjectDto? ToNullableProjectInternal(this Project? project);
}
