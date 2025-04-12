using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class IssueCreatedHandler
{
    public static async Task HandleAsync(IssueCreated created, AppDbContext db, ILogger logger, CancellationToken ct)
    {
        var data = JsonSerializer.SerializeToDocument(new { issueId = created.IssueId.Value });
        var users = await db
            .ProjectMembers.Where(a =>
                a.ProjectId == created.ProjectId && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssue))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.IssueCreated, Data = data };
        db.AddRange(users.Select(a => new UserNotification { UserId = a, Notification = notification }));
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
