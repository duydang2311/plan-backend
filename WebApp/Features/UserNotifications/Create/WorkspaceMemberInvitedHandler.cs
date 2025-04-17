using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public static class WorkspaceMemberInvitedHandler
{
    public static async Task<WorkspaceMemberInvitedUserNotified?> HandleAsync(
        WorkspaceMemberInvited invited,
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
                Type = NotificationType.WorkspaceMemberInvited,
                Data = JsonSerializer.SerializeToDocument(
                    new { workspaceInvitationId = invited.WorkspaceInvitationId.Value }
                ),
            },
        };
        db.Add(userNotification);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var invitation = await db
            .WorkspaceInvitations.Where(a => a.Id == invited.WorkspaceInvitationId)
            .Select(a => new { WorkspacePath = a.Workspace.Path, WorkspaceName = a.Workspace.Name })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        Console.WriteLine(
            "Create notif "
                + new WorkspaceMemberInvitedUserNotified
                {
                    UserId = invited.MemberId,
                    UserNotificationId = userNotification.Id,
                    Type = NotificationType.WorkspaceMemberInvited,
                    WorkspaceInvitationId = invited.WorkspaceInvitationId,
                    WorkspacePath = invitation.WorkspacePath,
                    WorkspaceName = invitation.WorkspaceName,
                }
        );
        return invitation is null
            ? null
            : new WorkspaceMemberInvitedUserNotified
            {
                UserId = invited.MemberId,
                UserNotificationId = userNotification.Id,
                Type = NotificationType.WorkspaceMemberInvited,
                WorkspaceInvitationId = invited.WorkspaceInvitationId,
                WorkspacePath = invitation.WorkspacePath,
                WorkspaceName = invitation.WorkspaceName,
            };
    }
}
