using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.GetMany;

public sealed record GetChecklistItemsHandler(AppDbContext db)
    : ICommandHandler<GetChecklistItems, PaginatedList<ChecklistItem>>
{
    public async Task<PaginatedList<ChecklistItem>> ExecuteAsync(GetChecklistItems command, CancellationToken ct)
    {
        var query = db.ChecklistItems.Where(a => a.ParentIssueId == command.ParentIssueId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (command.Cursor.HasValue)
        {
            query = query.Where(a => a.Id > command.Cursor.Value);
        }

        query = command.Order.SortOrDefault(query, a => a.OrderBy(b => b.Id));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ChecklistItem, ChecklistItem>(command.Select));
        }

        return PaginatedList.From(await query.Take(command.Size).ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
