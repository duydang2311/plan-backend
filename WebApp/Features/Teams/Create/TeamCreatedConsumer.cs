using Casbin;
using MassTransit;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.Create;

public sealed class TeamCreatedConsumer(AppDbContext dbContext, IEnforcer enforcer) : IConsumer<TeamCreated>
{
    public Task Consume(ConsumeContext<TeamCreated> context)
    {
        enforcer.AddPolicy(
            context.Message.UserId.ToString(),
            string.Empty,
            context.Message.Team.Id.ToString(),
            Permit.Read
        );
        dbContext.Add(new TeamMember { Team = context.Message.Team, MemberId = context.Message.UserId });
        return Task.CompletedTask;
    }
}
