using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamIssues.GetMany;

public sealed record GetTeamIssuesHandler(AppDbContext db) : ICommandHandler<GetTeamIssues, PaginatedList<TeamIssue>>
{
    public async Task<PaginatedList<TeamIssue>> ExecuteAsync(GetTeamIssues command, CancellationToken ct)
    {
        var query = db.TeamIssues.AsQueryable();

        if (command.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == command.TeamId);
        }

        if (command.IssueId.HasValue)
        {
            query = query.Where(x => x.IssueId == command.IssueId);
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command.Order.SortOrDefault(query, a => a.OrderBy(b => b.CreatedTime));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<TeamIssue, TeamIssue>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
