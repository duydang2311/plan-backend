using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class IssueCommentCreatedHandler
{
    public static async Task<IEnumerable<IssueCommentCreatedUserNotified>?> HandleAsync(
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
        var userNotifications = userIds
            .Select(a => new UserNotification { UserId = a, Notification = notification })
            .ToList();
        db.AddRange(userNotifications);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var issue = await db
            .Issues.Where(a => a.Id == created.IssueId)
            .Select(a => new
            {
                a.OrderNumber,
                a.Title,
                ProjectIdentifier = a.Project.Identifier,
                WorkspacePath = a.Project.Workspace.Path,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return issue is null
            ? null
            : userNotifications
                .Select(a => new IssueCommentCreatedUserNotified
                {
                    UserId = a.UserId,
                    UserNotificationId = a.Id,
                    Type = NotificationType.IssueCommentCreated,
                    OrderNumber = issue.OrderNumber,
                    Title = issue.Title,
                    ProjectIdentifier = issue.ProjectIdentifier,
                    WorkspacePath = issue.WorkspacePath,
                })
                .ToList();
    }
}
