using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public sealed class NotifyProjectCreatedJobHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<NotifyProjectCreatedJob>
{
    public async Task ExecuteAsync(NotifyProjectCreatedJob command, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = JsonSerializer.SerializeToDocument(new { projectId = command.ProjectId.Value });
        var users = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == command.WorkspaceId
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
