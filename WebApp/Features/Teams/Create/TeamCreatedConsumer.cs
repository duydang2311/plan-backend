using MassTransit;
using WebApp.Domain.Events;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Teams.Create;

public sealed class TeamCreatedConsumer(AppDbContext dbContext) : IConsumer<TeamCreated>
{
    public Task Consume(ConsumeContext<TeamCreated> context)
    {
        dbContext.Add(new TeamMember { Team = context.Message.Team, MemberId = context.Message.UserId });
        return Task.CompletedTask;
    }
}
