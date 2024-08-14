using WebApp.Common.Constants;

namespace WebApp.Domain.Constants;

public sealed record TeamRoleDefaults
{
    public int Id { get; }
    public string Name { get; }
    public string[] Permissions { get; }

    private TeamRoleDefaults(int id, string name, string[] permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }

    public static readonly TeamRoleDefaults Guest =
        new(1, "Guest", [Permit.ReadTeam, Permit.ReadIssue, Permit.ReadIssueComment]);
    public static readonly TeamRoleDefaults Member =
        new(2, "Member", [.. Guest.Permissions, Permit.CreateIssue, Permit.CommentIssue]);
    public static readonly TeamRoleDefaults Manager = new(3, "Manager", [.. Member.Permissions]);
    public static readonly TeamRoleDefaults Admin =
        new(4, "Administrator", [.. Manager.Permissions, Permit.UpdateTeamRole]);

    public static readonly TeamRoleDefaults[] Roles = [Guest, Member, Manager, Admin];
}
