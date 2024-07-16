using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.GetOne;

using Result = OneOf<None, Team>;

public sealed class GetTeamHandler(AppDbContext dbContext) : ICommandHandler<GetTeam, Result>
{
    public async Task<Result> ExecuteAsync(GetTeam command, CancellationToken ct)
    {
        var query = dbContext.Teams.AsQueryable();
        if (command.TeamId is not null)
        {
            query = query.Where(x => x.Id == command.TeamId);
        }
        else if (command.WorkspaceId is not null && command.Identifier is not null)
        {
            query = query.Where(x => x.WorkspaceId == command.WorkspaceId && x.Identifier.Equals(command.Identifier));
        }

        if (command.Select is not null)
        {
            query = query.Select<Team>(command.Select);
        }

        var team = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return team is null ? new None() : team;
    }
}
