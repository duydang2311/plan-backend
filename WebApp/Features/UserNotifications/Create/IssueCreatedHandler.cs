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
    public static async Task<IEnumerable<IssueCreatedUserNotified>?> HandleAsync(
        IssueCreated created,
        AppDbContext db,
        ILogger logger,
        CancellationToken ct
    )
    {
        var data = JsonSerializer.SerializeToDocument(new { issueId = created.IssueId.Value });
        var userIds = await db
            .ProjectMembers.Where(a =>
                a.ProjectId == created.ProjectId && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssue))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.IssueCreated, Data = data };
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
            : userNotifications.Select(a => new IssueCreatedUserNotified
            {
                UserId = a.UserId,
                UserNotificationId = a.Id,
                Type = NotificationType.IssueCreated,
                OrderNumber = issue.OrderNumber,
                Title = issue.Title,
                ProjectIdentifier = issue.ProjectIdentifier,
                WorkspacePath = issue.WorkspacePath,
            });
    }
}
