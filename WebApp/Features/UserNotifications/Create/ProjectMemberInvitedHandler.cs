using System.Text.Json;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class ProjectMemberInvitedHandler
{
    public static async Task HandleAsync(
        ProjectMemberInvited invited,
        AppDbContext db,
        ILogger logger,
        CancellationToken ct
    )
    {
        db.Add(
            new UserNotification
            {
                UserId = invited.MemberId,
                Notification = new Notification
                {
                    Type = NotificationType.ProjectMemberInvited,
                    Data = JsonSerializer.SerializeToDocument(
                        new { projectMemberInvitationId = invited.ProjectMemberInvitationId.Value }
                    ),
                },
            }
        );
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
