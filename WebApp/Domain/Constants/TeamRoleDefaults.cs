using WebApp.Common.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Constants;

public sealed record TeamRoleDefaults
{
    public TeamRoleId Id { get; }
    public string Name { get; }
    public string[] Permissions { get; }

    private TeamRoleDefaults(TeamRoleId id, string name, string[] permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }

    public static readonly TeamRoleDefaults Guest =
        new(new TeamRoleId { Value = 100 }, "Guest", [Permit.ReadTeam, Permit.ReadIssue, Permit.ReadIssueComment]);
    public static readonly TeamRoleDefaults Member =
        new(new TeamRoleId { Value = 200 }, "Member", [.. Guest.Permissions, Permit.CreateIssue, Permit.CommentIssue]);
    public static readonly TeamRoleDefaults Manager =
        new(new TeamRoleId { Value = 300 }, "Manager", [.. Member.Permissions, Permit.ReadTeamInvitation]);
    public static readonly TeamRoleDefaults Admin =
        new(
            new TeamRoleId { Value = 400 },
            "Administrator",
            [.. Manager.Permissions, Permit.UpdateTeamRole, Permit.CreateTeamMember]
        );

    public static readonly TeamRoleDefaults[] Roles = [Guest, Member, Manager, Admin];
}
