using Casbin;
using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.Create;

public sealed class TeamCreatedHandler : IEventHandler<TeamCreated>
{
    public Task HandleAsync(TeamCreated eventModel, CancellationToken ct)
    {
        var (dbContext, enforcer) = (
            eventModel.ServiceProvider.GetRequiredService<AppDbContext>(),
            eventModel.ServiceProvider.GetRequiredService<IEnforcer>()
        );
        var sUserId = eventModel.UserId.ToString();
        var sTeamId = eventModel.Team.Id.ToString();
        enforcer.AddPolicy(sUserId, string.Empty, sTeamId, Permit.Read);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.Read);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.CreateIssue);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.CommentIssue);
        enforcer.AddGroupingPolicy(sUserId, "member", sTeamId);
        dbContext.Add(
            new TeamMember
            {
                Team = eventModel.Team,
                MemberId = eventModel.UserId,
                RoleId = new TeamRoleId { Value = TeamRoleDefaults.Admin.Id }
            }
        );
        dbContext.Add(new SharedCounter { Id = eventModel.Team.Id.Value });
        return Task.CompletedTask;
    }
}
