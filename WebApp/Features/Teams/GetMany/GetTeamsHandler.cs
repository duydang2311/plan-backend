using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Teams.GetMany;

using Result = PaginatedList<Team>;

public sealed class GetTeamsHandler(AppDbContext dbContext) : ICommandHandler<GetTeams, Result>
{
    public async Task<Result> ExecuteAsync(GetTeams command, CancellationToken ct)
    {
        var query = dbContext.Teams.AsQueryable();
        if (command.WorkspaceId is not null)
        {
            query = query.Where(x => x.WorkspaceId == command.WorkspaceId);
        }
        if (command.UserId is not null)
        {
            query = query.Where(x => x.Members.Any(x => x.Id == command.UserId));
        }
        if (command.Select is not null)
        {
            query = query.Select(ExpressionHelper.Select<Team, Team>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query = command
            .Order.Where(static x =>
                x.Name.EqualsEither(
                    ["CreatedTime", "UpdatedTime", "Name", "Identifier"],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .SortOrDefault(query, x => x.OrderByDescending(x => x.CreatedTime));
        var teams = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { Items = teams, TotalCount = totalCount };
    }
}
