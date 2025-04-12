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
    public static async Task HandleAsync(ProjectCreated created, AppDbContext db, ILogger logger, CancellationToken ct)
    {
        var data = JsonSerializer.SerializeToDocument(new { projectId = created.ProjectId.Value });
        var users = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == created.WorkspaceId
                && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadProject))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.ProjectCreated, Data = data };
        db.AddRange(users.Select(a => new UserNotification { UserId = a, Notification = notification }));
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
