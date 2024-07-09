using FastEndpoints;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Domain.Events;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Teams.Create;

using Result = OneOf<ValidationFailures, Team>;

public sealed class CreateTeamHandler(AppDbContext dbContext, IScopedMediator mediator)
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

        await mediator.Publish(new TeamCreated { Team = team, UserId = command.UserId }, ct).ConfigureAwait(true);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(true);

        return team;
    }
}
