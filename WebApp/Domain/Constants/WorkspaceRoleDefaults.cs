using WebApp.Common.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public sealed record WorkspaceRoleDefaults
{
    public RoleId Id { get; }
    public string Name { get; }
    public string[] Permissions { get; }

    private WorkspaceRoleDefaults(RoleId id, string name, string[] permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }

    public static readonly WorkspaceRoleDefaults Guest = new(
        new RoleId { Value = 1100 },
        "Guest",
        [Permit.ReadProject, Permit.ReadTeam]
    );
    public static readonly WorkspaceRoleDefaults Member = new(
        new RoleId { Value = 1200 },
        "Member",
        [.. Guest.Permissions, Permit.CreateIssue, Permit.ReadProjectMemberInvitation]
    );
    public static readonly WorkspaceRoleDefaults Manager = new(
        new RoleId { Value = 1300 },
        "Manager",
        [
            .. Member.Permissions,
            Permit.CreateProject,
            Permit.CreateWorkspaceStatus,
            Permit.DeleteProject,
            Permit.CreateTeam,
            Permit.DeleteProjectMember,
            Permit.CreateProjectMember,
            Permit.CreateProjectMemberInvitation,
            Permit.DeleteProjectMemberInvitation,
        ]
    );
    public static readonly WorkspaceRoleDefaults Admin = new(
        new RoleId { Value = 1400 },
        "Administrator",
        [.. Manager.Permissions, Permit.DeleteWorkspaceMember]
    );

    public static readonly WorkspaceRoleDefaults[] Roles = [Guest, Member, Manager, Admin];
}
