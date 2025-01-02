using FastEndpoints;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.Create;

public sealed class TeamCreatedHandler : IEventHandler<TeamCreated>
{
    public Task HandleAsync(TeamCreated eventModel, CancellationToken ct)
    {
        var dbContext = eventModel.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Add(
            new TeamMember
            {
                Team = eventModel.Team,
                MemberId = eventModel.UserId,
                RoleId = TeamRoleDefaults.Admin.Id
            }
        );
        dbContext.Add(new SharedCounter { Id = eventModel.Team.Id.Value });
        return Task.CompletedTask;
    }
}
