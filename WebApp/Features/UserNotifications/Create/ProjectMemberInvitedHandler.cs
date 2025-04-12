using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class ProjectMemberInvitedHandler
{
    public static async Task<ProjectMemberInvitedUserNotified?> HandleAsync(
        ProjectMemberInvited invited,
        AppDbContext db,
        ILogger logger,
        CancellationToken ct
    )
    {
        var userNotification = new UserNotification
        {
            UserId = invited.MemberId,
            Notification = new Notification
            {
                Type = NotificationType.ProjectMemberInvited,
                Data = JsonSerializer.SerializeToDocument(
                    new { projectMemberInvitationId = invited.ProjectMemberInvitationId.Value }
                ),
            },
        };
        db.Add(userNotification);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var invitation = await db
            .ProjectMemberInvitations.Where(a => a.Id == invited.ProjectMemberInvitationId)
            .Select(a => new
            {
                a.Id,
                ProjectIdentifier = a.Project.Identifier,
                ProjectName = a.Project.Name,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        return invitation is null
            ? null
            : new ProjectMemberInvitedUserNotified
            {
                UserId = invited.MemberId,
                UserNotificationId = userNotification.Id,
                Type = NotificationType.ProjectMemberInvited,
                ProjectMemberInvitationId = invitation.Id,
                ProjectIdentifier = invitation.ProjectIdentifier,
                ProjectName = invitation.ProjectName,
            };
    }
}
