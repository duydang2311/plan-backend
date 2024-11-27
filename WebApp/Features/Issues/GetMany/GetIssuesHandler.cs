using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.GetMany;

public sealed class GetIssuesHandler(AppDbContext dbContext) : ICommandHandler<GetIssues, PaginatedList<Issue>>
{
    public async Task<PaginatedList<Issue>> ExecuteAsync(GetIssues command, CancellationToken ct)
    {
        var query = dbContext.Issues.AsQueryable();
        if (!command.TeamId.HasValue && !command.ProjectId.HasValue)
        {
            query = query.Where(a =>
                a.Team.Members.Any(b => b.Id == command.UserId)
                || a.ProjectIssues.Any(b => b.Project.Teams.Any(b => b.Members.Any(c => c.Id == command.UserId)))
            );
        }
        else
        {
            if (command.TeamId.HasValue)
            {
                query = query.Where(a => a.TeamId == command.TeamId);
            }
            if (command.ProjectId.HasValue)
            {
                query = query.Where(i =>
                    i.ProjectIssues.Any(p =>
                        p.ProjectId == command.ProjectId
                        && p.Project.Teams.Any(t => t.Members.Any(m => m.Id == command.UserId))
                    )
                );
            }
        }

        if (command.StatusId.HasValue)
        {
            query = query.Where(a => a.StatusId == command.StatusId);
        }
        else if (command.NullStatusId == true)
        {
            query = query.Where(a => a.StatusId == null);
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Issue, Issue>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query = command
            .Order.Where(static x =>
                x.Name.EqualsEither(
                    ["CreatedTime", "UpdatedTime", "Title", "OrderNumber", "Priority", "Status.Rank"],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .SortOrDefault(query, x => x.OrderByDescending(x => x.CreatedTime));
        var issues = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { Items = issues, TotalCount = totalCount, };
    }
}
