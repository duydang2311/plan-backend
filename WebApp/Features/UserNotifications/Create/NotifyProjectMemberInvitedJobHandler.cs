using System.Text.Json;
using FastEndpoints;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public sealed class NotifyProjectMemberInvitedJobHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<NotifyProjectMemberInvitedJob>
{
    public async Task ExecuteAsync(NotifyProjectMemberInvitedJob command, CancellationToken ct)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Add(
            new UserNotification
            {
                UserId = command.MemberId,
                Notification = new Notification
                {
                    Type = NotificationType.ProjectMemberInvited,
                    Data = JsonSerializer.SerializeToDocument(
                        new { projectMemberInvitationId = command.ProjectMemberInvitationId.Value }
                    ),
                },
            }
        );
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
