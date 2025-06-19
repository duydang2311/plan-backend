using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class IssueStatusUpdatedHandler
{
    public static async Task<IEnumerable<IssueStatusUpdatedUserNotified>?> HandleAsync(
        IssueStatusUpdated updated,
        AppDbContext db,
        CancellationToken ct
    )
    {
        var project = await db
            .Projects.Where(a => a.Id == updated.ProjectId)
            .Select(a => new { a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (project is null)
        {
            return null;
        }

        var statusIds = new List<StatusId>(2);
        if (updated.OldStatusId.HasValue)
        {
            statusIds.Add(updated.OldStatusId.Value);
        }
        if (updated.NewStatusId.HasValue)
        {
            statusIds.Add(updated.NewStatusId.Value);
        }

        var data = JsonSerializer.SerializeToDocument(
            new
            {
                issueId = updated.IssueId.Value,
                oldStatusId = updated.OldStatusId.HasValue ? updated.OldStatusId.Value.Value : (long?)null,
                newStatusId = updated.NewStatusId.HasValue ? updated.NewStatusId.Value.Value : (long?)null,
            },
            NotificationDefaults.JsonSerializerOptions
        );

        var userIds = await db
            .ProjectMembers.Where(a =>
                a.ProjectId == updated.ProjectId && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssue))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        var notification = new Notification { Type = NotificationType.IssueStatusUpdated, Data = data };
        var userNotifications = userIds
            .Select(a => new UserNotification { UserId = a, Notification = notification })
            .ToList();
        db.AddRange(userNotifications);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var statuses = await db
            .Statuses.Where(a => statusIds.Contains(a.Id))
            .Select(a => new
            {
                a.Id,
                a.Value,
                a.Category,
                a.Color,
            })
            .ToDictionaryAsync(a => a.Id, ct)
            .ConfigureAwait(false);

        var issue = await db
            .Issues.Where(a => a.Id == updated.IssueId)
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
            : userNotifications.Select(a => new IssueStatusUpdatedUserNotified
            {
                UserId = a.UserId,
                UserNotificationId = a.Id,
                Type = NotificationType.IssueStatusUpdated,
                OrderNumber = issue.OrderNumber,
                Title = issue.Title,
                ProjectIdentifier = issue.ProjectIdentifier,
                WorkspacePath = issue.WorkspacePath,
                OldStatusCategory = updated.OldStatusId.HasValue
                    ? (byte?)statuses.GetValueOrDefault(updated.OldStatusId.Value)?.Category
                    : null,
                OldStatusColor = updated.OldStatusId.HasValue
                    ? statuses.GetValueOrDefault(updated.OldStatusId.Value)?.Color
                    : null,
                OldStatusValue = updated.OldStatusId.HasValue
                    ? statuses.GetValueOrDefault(updated.OldStatusId.Value)?.Value
                    : null,
                NewStatusCategory = updated.NewStatusId.HasValue
                    ? (byte?)statuses.GetValueOrDefault(updated.NewStatusId.Value)?.Category
                    : null,
                NewStatusColor = updated.NewStatusId.HasValue
                    ? statuses.GetValueOrDefault(updated.NewStatusId.Value)?.Color
                    : null,
                NewStatusValue = updated.NewStatusId.HasValue
                    ? statuses.GetValueOrDefault(updated.NewStatusId.Value)?.Value
                    : null,
            });
    }
}
