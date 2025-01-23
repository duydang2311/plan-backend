using WebApp.Common.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public sealed record ProjectRoleDefaults
{
    public RoleId Id { get; }
    public string Name { get; }
    public string[] Permissions { get; }

    private ProjectRoleDefaults(RoleId id, string name, string[] permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }

    public static readonly ProjectRoleDefaults Guest =
        new(new RoleId { Value = 2100 }, "Guest", [Permit.ReadIssue, Permit.ReadIssueAudit, Permit.ReadProjectMember]);
    public static readonly ProjectRoleDefaults Member =
        new(
            new RoleId { Value = 2200 },
            "Member",
            [.. Guest.Permissions, Permit.CreateIssue, Permit.ReadProjectMemberInvitation]
        );
    public static readonly ProjectRoleDefaults Manager =
        new(
            new RoleId { Value = 2300 },
            "Manager",
            [
                .. Member.Permissions,
                Permit.DeleteProjectMember,
                Permit.CreateProjectMember,
                Permit.CreateProjectMemberInvitation,
                Permit.DeleteProjectMemberInvitation
            ]
        );
    public static readonly ProjectRoleDefaults Admin =
        new(new RoleId { Value = 2400 }, "Administrator", [.. Manager.Permissions]);

    public static readonly ProjectRoleDefaults[] Roles = [Guest, Member, Manager, Admin];
}
