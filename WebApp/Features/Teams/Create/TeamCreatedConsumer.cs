using Casbin;
using MassTransit;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.Create;

public sealed class TeamCreatedConsumer(AppDbContext dbContext, IEnforcer enforcer) : IConsumer<TeamCreated>
{
    public Task Consume(ConsumeContext<TeamCreated> context)
    {
        var sUserId = context.Message.UserId.ToString();
        var sTeamId = context.Message.Team.Id.ToString();
        enforcer.AddPolicy(sUserId, string.Empty, sTeamId, Permit.Read);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.Read);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.CreateIssue);
        enforcer.AddPolicy("member", sTeamId, sTeamId, Permit.CommentIssue);
        enforcer.AddGroupingPolicy(sUserId, "member", sTeamId);
        dbContext.Add(
            new TeamMember
            {
                Team = context.Message.Team,
                MemberId = context.Message.UserId,
                RoleId = new TeamRoleId { Value = TeamRoleDefaults.Admin.Id }
            }
        );
        return Task.CompletedTask;
    }
}
