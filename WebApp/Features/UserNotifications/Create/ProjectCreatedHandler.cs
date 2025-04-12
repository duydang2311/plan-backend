using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class ProjectCreatedHandler
{
    public static async Task<IEnumerable<ProjectCreatedUserNotified>?> HandleAsync(
        ProjectCreated created,
        AppDbContext db,
        ILogger logger,
        CancellationToken ct
    )
    {
        var data = JsonSerializer.SerializeToDocument(new { projectId = created.ProjectId.Value });
        var userIds = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == created.WorkspaceId
                && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadProject))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.ProjectCreated, Data = data };
        var userNotifications = userIds
            .Select(a => new UserNotification { UserId = a, Notification = notification })
            .ToList();
        db.AddRange(userNotifications);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var project = await db
            .Projects.Where(a => a.Id == created.ProjectId)
            .Select(a => new
            {
                a.Identifier,
                a.Name,
                WorkspacePath = a.Workspace.Path,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return project is null
            ? null
            : userNotifications.Select(a => new ProjectCreatedUserNotified
            {
                UserId = a.UserId,
                UserNotificationId = a.Id,
                Type = NotificationType.ProjectCreated,
                Identifier = project.Identifier,
                Name = project.Name,
                WorkspacePath = project.WorkspacePath,
            });
    }
}
