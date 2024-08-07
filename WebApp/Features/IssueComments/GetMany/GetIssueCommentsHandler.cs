using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.GetMany;

public sealed class GetIssueCommentsHandler(AppDbContext dbContext)
    : ICommandHandler<GetIssueComments, PaginatedList<IssueComment>>
{
    public async Task<PaginatedList<IssueComment>> ExecuteAsync(GetIssueComments command, CancellationToken ct)
    {
        var query = dbContext.IssueComments.AsQueryable();
        if (command.IssueId.HasValue)
        {
            query = query.Where(x => x.IssueId == command.IssueId.Value);
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = query.OrderBy(x => x.CreatedTime);
        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);

        return new() { Items = items, TotalCount = totalCount, };
    }
}
