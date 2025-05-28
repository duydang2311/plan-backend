using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.MilestoneStatuses.GetMany;

public sealed record GetMilestoneStatusesHandler(AppDbContext db)
    : ICommandHandler<GetMilestoneStatuses, PaginatedList<MilestoneStatus>>
{
    public async Task<PaginatedList<MilestoneStatus>> ExecuteAsync(GetMilestoneStatuses command, CancellationToken ct)
    {
        var query = db.MilestoneStatuses.Where(m => m.ProjectId == command.ProjectId).AsQueryable();
        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.Id));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<MilestoneStatus, MilestoneStatus>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
