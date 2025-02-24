using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public sealed class NotifyIssueCommentCreatedJobHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<NotifyIssueCommentCreatedJob>
{
    public async Task ExecuteAsync(NotifyIssueCommentCreatedJob command, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = JsonSerializer.SerializeToDocument(new { issueAuditId = command.IssueAuditId });
        var userIds = await db
            .IssueAssignees.Where(a => a.IssueId == command.IssueId)
            .Select(a => a.UserId)
            .Union(
                db.IssueAudits.Where(a => a.IssueId == command.IssueId && a.Action == IssueAuditAction.Comment)
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
