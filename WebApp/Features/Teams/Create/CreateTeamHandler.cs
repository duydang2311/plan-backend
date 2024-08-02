using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.Create;

using Result = OneOf<ValidationFailures, Team>;

public sealed class CreateTeamHandler(AppDbContext dbContext, IServiceProvider serviceProvider)
    : ICommandHandler<CreateTeam, Result>
{
    public async Task<Result> ExecuteAsync(CreateTeam command, CancellationToken ct)
    {
        if (!await dbContext.Workspaces.AnyAsync(x => x.Id == command.WorkspaceId, ct).ConfigureAwait(false))
        {
            return ValidationFailures.Single("workspaceId", "Workspace does not exist", "not_found");
        }

        if (
            await dbContext
                .Teams.AnyAsync(
                    x => x.WorkspaceId == command.WorkspaceId && x.Identifier.Equals(command.Identifier),
                    ct
                )
                .ConfigureAwait(false)
        )
        {
            return ValidationFailures.Single("identifier", "Identifier has already existed", "duplicated");
        }

        var team = new Team
        {
            WorkspaceId = command.WorkspaceId,
            Identifier = command.Identifier,
            Name = command.Name,
        };
        dbContext.Add(team);

        await new TeamCreated
        {
            ServiceProvider = serviceProvider,
            Team = team,
            UserId = command.UserId
        }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(true);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(true);

        return team;
    }
}
