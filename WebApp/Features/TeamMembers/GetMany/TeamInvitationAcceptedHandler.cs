using FastEndpoints;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamMembers.GetMany;

public sealed class TeamInvitationAcceptedHandler : IEventHandler<TeamInvitationAccepted>
{
    public Task HandleAsync(TeamInvitationAccepted eventModel, CancellationToken ct)
    {
        var db = eventModel.ServiceProvider.GetRequiredService<AppDbContext>();
        db.TeamMembers.Add(
            new TeamMember
            {
                MemberId = eventModel.TeamInvitation.MemberId,
                TeamId = eventModel.TeamInvitation.TeamId,
                RoleId = TeamRoleDefaults.Member.Id
            }
        );
        return Task.CompletedTask;
    }
}
