using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.GetMany;

public sealed class GetIssuesHandler(AppDbContext dbContext) : ICommandHandler<GetIssues, PaginatedList<Issue>>
{
    public async Task<PaginatedList<Issue>> ExecuteAsync(GetIssues command, CancellationToken ct)
    {
        var query = dbContext.Issues.AsQueryable();
        if (command.TeamId is not null)
        {
            query = query.Where(x => x.TeamId == command.TeamId);
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select<Issue>(command.Select);
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query = command
            .Order.Where(static x =>
                x.Name.EqualsEither(
                    ["CreatedTime", "UpdatedTime", "Title", "OrderNumber"],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .SortOrDefault(query, x => x.OrderByDescending(x => x.CreatedTime));
        var issues = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { Items = issues, TotalCount = totalCount, };
    }
}
