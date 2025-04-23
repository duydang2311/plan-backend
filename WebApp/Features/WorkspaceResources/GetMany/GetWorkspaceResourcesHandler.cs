using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.GetMany;

public sealed record GetWorkspaceResourcesHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceResources, PaginatedList<WorkspaceResource>>
{
    public async Task<PaginatedList<WorkspaceResource>> ExecuteAsync(
        GetWorkspaceResources command,
        CancellationToken ct
    )
    {
        var query = db.WorkspaceResources.Where(a => a.WorkspaceId == command.WorkspaceId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (command.Cursor.HasValue)
        {
            query = query.Where(a => a.ResourceId < command.Cursor.Value);
        }

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.ResourceId));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceResource, WorkspaceResource>(command.Select));
        }

        return PaginatedList.From(await query.Take(command.Size).ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
