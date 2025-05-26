using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Milestones.GetMany;

public sealed record GetMilestonesHandler(AppDbContext db) : ICommandHandler<GetMilestones, PaginatedList<Milestone>>
{
    public async Task<PaginatedList<Milestone>> ExecuteAsync(GetMilestones command, CancellationToken ct)
    {
        var query = db.Milestones.Where(m => m.ProjectId == command.ProjectId).AsQueryable();
        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.Id));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Milestone, Milestone>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
