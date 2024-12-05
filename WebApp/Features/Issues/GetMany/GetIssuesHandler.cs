using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
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
                a.Project.Members.Any(b =>
                    b.UserId == command.UserId && b.Role.Permissions.Any(c => c.Permission.Equals(Permit.ReadIssue))
                )
                || a.Teams.Any(b =>
                    b.Members.Any(b =>
                        b.Id == command.UserId
                        && b.Roles.Any(c => c.Role.Permissions.Any(d => d.Permission.Equals(Permit.ReadIssue)))
                    )
                )
            );
        }
        else
        {
            if (command.ProjectId.HasValue)
            {
                query = query.Where(a => a.ProjectId == command.ProjectId.Value);
            }
            if (command.TeamId.HasValue)
            {
                query = query.Where(a => a.Teams.Any(b => b.Id == command.TeamId.Value));
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
                    ["CreatedTime", "UpdatedTime", "Title", "OrderNumber", "Priority", "StatusRank", "Status.Rank"],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .SortOrDefault(query, x => x.OrderByDescending(x => x.CreatedTime));
        var issues = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { Items = issues, TotalCount = totalCount, };
    }
}
