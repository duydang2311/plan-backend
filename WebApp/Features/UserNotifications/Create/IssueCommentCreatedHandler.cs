using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class IssueCommentCreatedHandler
{
    public static async Task HandleAsync(
        IssueCommentCreated created,
        AppDbContext db,
        ILogger logger,
        CancellationToken ct
    )
    {
        var data = JsonSerializer.SerializeToDocument(new { issueAuditId = created.IssueAuditId });
        var userIds = await db
            .IssueAssignees.Where(a => a.IssueId == created.IssueId)
            .Select(a => a.UserId)
            .Union(
                db.IssueAudits.Where(a => a.IssueId == created.IssueId && a.Action == IssueAuditAction.Comment)
                    .Select(a => a.UserId)
                    .Cast<UserId>()
            )
            .Distinct()
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.IssueCommentCreated, Data = data };
        db.AddRange(userIds.Select(a => new UserNotification { UserId = a, Notification = notification }));
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
