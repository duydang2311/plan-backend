using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.Create;

public sealed class TeamInvitationAcceptedHandler : IEventHandler<TeamInvitationAccepted>
{
    public async Task HandleAsync(TeamInvitationAccepted eventModel, CancellationToken ct)
    {
        var db = eventModel.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = await db
            .Teams.Where(a => a.Id == eventModel.TeamInvitation.TeamId)
            .Select(a => new
            {
                a.WorkspaceId,
                IsWorkspaceMember = a.Workspace.Members.Any(a => a.UserId == eventModel.TeamInvitation.MemberId)
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (data is not null && !data.IsWorkspaceMember)
        {
            db.Add(
                new WorkspaceMember
                {
                    RoleId = WorkspaceRoleDefaults.Guest.Id,
                    UserId = eventModel.TeamInvitation.MemberId,
                    WorkspaceId = data.WorkspaceId
                }
            );
        }
    }
}
