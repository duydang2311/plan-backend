using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public sealed class NotifyIssueCreatedJobHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<NotifyIssueCreatedJob>
{
    public async Task ExecuteAsync(NotifyIssueCreatedJob command, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = JsonSerializer.SerializeToDocument(new { issueId = command.IssueId.Value });
        var users = await db
            .ProjectMembers.Where(a =>
                a.ProjectId == command.ProjectId && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadIssue))
            )
            .Select(a => a.UserId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        var notification = new Notification { Type = NotificationType.IssueCreated, Data = data };
        db.AddRange(users.Select(a => new UserNotification { UserId = a, Notification = notification }));
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
