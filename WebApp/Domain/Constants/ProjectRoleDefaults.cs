using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Domain.Constants;

public sealed record ProjectRoleDefaults
{
    public RoleId Id { get; }
    public string Name { get; }
    public int Rank { get; }
    public string[] Permissions { get; }

    private ProjectRoleDefaults(RoleId id, string name, int rank, string[] permissions)
    {
        Id = id;
        Name = name;
        Rank = rank;
        Permissions = permissions;
    }

    public static readonly ProjectRoleDefaults Guest = new(RoleId.From(16), "Guest", 500, [Permit.ReadIssue]);
    public static readonly ProjectRoleDefaults Viewer = new(
        RoleId.From(15),
        "Viewer",
        400,
        [
            .. Guest.Permissions,
            Permit.ReadProjectMember,
            Permit.ReadProjectMemberInvitation,
            Permit.ReadIssueAudit,
            Permit.ReadProjectResourceFile,
            Permit.ReadIssueAssignee,
            Permit.ReadTeamIssue,
        ]
    );
    public static readonly ProjectRoleDefaults Member = new(
        RoleId.From(14),
        "Member",
        300,
        [.. Viewer.Permissions, Permit.CreateIssue, Permit.CreateProjectResource, Permit.CreateProjectResourceFile]
    );
    public static readonly ProjectRoleDefaults Manager = new(
        RoleId.From(13),
        "Manager",
        200,
        [
            .. Member.Permissions,
            Permit.CreateProjectMember,
            Permit.CreateProjectMemberInvitation,
            Permit.CreateIssueAssignee,
            Permit.CreateTeamIssue,
            Permit.CreateIssueAuditComment,
            Permit.UpdateIssue,
            Permit.UpdateProjectResource,
            Permit.UpdateProjectResourceFile,
            Permit.DeleteProjectMember,
            Permit.DeleteProjectResource,
            Permit.DeleteProjectResourceFile,
            Permit.DeleteProjectMemberInvitation,
            Permit.UpdateProjectMember,
            Permit.DeleteIssue,
            Permit.DeleteIssueAssignee,
            Permit.DeleteTeamIssue,
        ]
    );
    public static readonly ProjectRoleDefaults Admin = new(
        RoleId.From(12),
        "Administrator",
        100,
        [.. Manager.Permissions]
    );
    public static readonly ProjectRoleDefaults Owner = new(RoleId.From(11), "Owner", 0, [.. Manager.Permissions]);

    public static readonly ProjectRoleDefaults[] Roles = [Guest, Member, Manager, Admin, Owner];

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
