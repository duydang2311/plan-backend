using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectIssues.GetMany;

public sealed class GetProjectIssuesHandler(AppDbContext dbContext)
    : ICommandHandler<GetProjectIssues, OneOf<PaginatedList<ProjectIssue>>>
{
    public async Task<OneOf<PaginatedList<ProjectIssue>>> ExecuteAsync(GetProjectIssues command, CancellationToken ct)
    {
        var query = dbContext.ProjectIssues.Where(pi =>
            pi.ProjectId == command.ProjectId && pi.Project.Teams.Any(t => t.Members.Any(m => m.Id == command.UserId))
        );

        if (command.StatusId.HasValue)
        {
            query = query.Where(a => a.Issue.StatusId == command.StatusId);
        }
        else if (command.NullStatusId == true)
        {
            query = query.Where(a => a.Issue.StatusId == null);
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ProjectIssue, ProjectIssue>(command.Select));
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

        return new PaginatedList<ProjectIssue>() { Items = issues, TotalCount = totalCount, };
    }
}
