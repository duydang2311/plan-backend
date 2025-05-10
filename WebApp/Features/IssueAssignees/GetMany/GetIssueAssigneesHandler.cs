using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueAssignees.GetMany;

public sealed record GetIssueAssigneesHandler(AppDbContext db)
    : ICommandHandler<GetIssueAssignees, PaginatedList<IssueAssignee>>
{
    public async Task<PaginatedList<IssueAssignee>> ExecuteAsync(GetIssueAssignees command, CancellationToken ct)
    {
        var query = db.IssueAssignees.Where(x => x.IssueId == command.IssueId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command.Order.SortOrDefault(query, a => a.OrderBy(b => b.CreatedTime));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<IssueAssignee, IssueAssignee>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
