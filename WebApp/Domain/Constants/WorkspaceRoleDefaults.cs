using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Domain.Constants;

public sealed record WorkspaceRoleDefaults
{
    public RoleId Id { get; }
    public string Name { get; }
    public int Rank { get; }
    public string[] Permissions { get; }

    private WorkspaceRoleDefaults(RoleId id, string name, int rank, string[] permissions)
    {
        Id = id;
        Name = name;
        Rank = rank;
        Permissions = permissions;
    }

    public static readonly WorkspaceRoleDefaults Guest = new(
        RoleId.From(6),
        "Guest",
        500,
        [Permit.ReadProject, Permit.ReadTeam, Permit.ReadWorkspaceMember, Permit.ReadWorkspaceStatus]
    );

    public static readonly WorkspaceRoleDefaults Viewer = new(
        RoleId.From(5),
        "Viewer",
        400,
        [
            .. Guest.Permissions,
            Permit.ReadIssue,
            Permit.ReadIssueAudit,
            Permit.ReadProjectMember,
            Permit.ReadWorkspaceResource,
            Permit.ReadWorkspaceInvitation,
            Permit.ReadProjectMemberInvitation,
            Permit.ReadIssueAssignee,
            Permit.ReadTeamIssue,
            Permit.ReadChecklistItem,
            Permit.ReadMilestone,
            Permit.ReadMilestoneStatus,
        ]
    );

    public static readonly WorkspaceRoleDefaults Member = new(
        RoleId.From(4),
        "Member",
        300,
        [
            .. Viewer.Permissions,
            Permit.ReadWorkspaceResourceFile,
            Permit.CreateIssue,
            Permit.CreateWorkspaceResource,
            Permit.CreateWorkspaceResourceFile,
        ]
    );

    public static readonly WorkspaceRoleDefaults Manager = new(
        RoleId.From(3),
        "Manager",
        200,
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
            Permit.UpdateWorkspaceStatus,
            Permit.DeleteWorkspaceStatus,
            Permit.UpdateWorkspaceResource,
            Permit.DeleteWorkspaceResource,
            Permit.CreateWorkspaceInvitation,
            Permit.UpdateWorkspaceInvitation,
            Permit.DeleteWorkspaceInvitation,
            Permit.CreateIssueAssignee,
            Permit.CreateTeamIssue,
            Permit.DeleteIssueAssignee,
            Permit.DeleteTeamIssue,
            Permit.DeleteWorkspaceResourceFile,
            Permit.UpdateWorkspaceMember,
            Permit.DeleteWorkspaceMember,
            Permit.CreateChecklistItem,
            Permit.DeleteChecklistItem,
            Permit.UpdateChecklistItem,
            Permit.CreateMilestone,
            Permit.UpdateMilestone,
            Permit.DeleteMilestone,
            Permit.CreateMilestoneStatus,
            Permit.UpdateMilestoneStatus,
            Permit.DeleteMilestoneStatus,
        ]
    );

    public static readonly WorkspaceRoleDefaults Admin = new(
        RoleId.From(2),
        "Administrator",
        100,
        [.. Manager.Permissions]
    );

    public static readonly WorkspaceRoleDefaults Owner = new(RoleId.From(1), "Owner", 0, [.. Admin.Permissions]);

    public static readonly WorkspaceRoleDefaults[] Roles = [Guest, Member, Manager, Admin, Owner];

    public static async Task SeedAsync(AppDbContext db, CancellationToken ct)
    {
        var roleIds = Roles.Select(a => a.Id).ToArray();
        var existingRoles = await db
            .Roles.Where(a => roleIds.Contains(a.Id))
            .Select(a => a.Id)
            .ToArrayAsync(ct)
            .ConfigureAwait(false);
        var newRoles = Roles.Where(a => !existingRoles.Contains(a.Id)).ToArray();
        var shouldSave = false;

        if (newRoles.Length > 0)
        {
            await db
                .Roles.AddRangeAsync(
                    newRoles.Select(a => new Role
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Rank = a.Rank,
                        Permissions = [.. a.Permissions.Select(b => new RolePermission { Permission = b })],
                    }),
                    ct
                )
                .ConfigureAwait(false);
            shouldSave = true;
        }

        // for roles that are already in database, check which permissions are missing and add them
        var existingRolePermissions = await db
            .Roles.Where(a => roleIds.Contains(a.Id))
            .Select(a => new { a.Id, Permissions = a.Permissions.Select(b => b.Permission) })
            .ToArrayAsync(ct)
            .ConfigureAwait(false);
        if (existingRolePermissions.Length > 0)
        {
            // add which permission in permissions array is missing in the database
            var missingPermissions = Roles
                .Select(a => new
                {
                    a.Id,
                    MissingPermissions = a.Permissions.Except(
                        existingRolePermissions.First(b => b.Id == a.Id).Permissions
                    ),
                })
                .Where(a => a.MissingPermissions.Any())
                .SelectMany(a => a.MissingPermissions.Select(b => new RolePermission { RoleId = a.Id, Permission = b }))
                .ToArray();
            db.AddRange(missingPermissions);
            shouldSave = true;
        }

        if (shouldSave)
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}
