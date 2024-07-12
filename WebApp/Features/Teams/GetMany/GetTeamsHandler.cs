using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

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
            query = query.Select<Team>(command.Select);
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query =
            Array
                .Find(command.Order, x => x.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                ?.Sort(query, x => x.Name) ?? query;
        query =
            Array
                .Find(command.Order, x => x.Name.Equals("identifier", StringComparison.OrdinalIgnoreCase))
                ?.Sort(query, x => x.Identifier) ?? query;
        query = command
            .Order.Where(x => !x.Name.EqualsEither(["name", "identifier"], StringComparison.OrdinalIgnoreCase))
            .SortOrDefault(query, x => x.OrderBy(x => x.CreatedTime));
        var teams = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { TotalCount = totalCount, Items = teams };
    }
}
