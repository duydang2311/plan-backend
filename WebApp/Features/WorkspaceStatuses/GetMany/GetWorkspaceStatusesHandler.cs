using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceStatuses.GetMany;

public sealed class GetWorkspaceStatusesHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceStatuses, OneOf<PaginatedList<WorkspaceStatus>>>
{
    public async Task<OneOf<PaginatedList<WorkspaceStatus>>> ExecuteAsync(
        GetWorkspaceStatuses command,
        CancellationToken ct
    )
    {
        var query = db.WorkspaceStatuses.Where(a => a.WorkspaceId == command.WorkspaceId);
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceStatus, WorkspaceStatus>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query = command
            .Order.Where(a => a.Name.EqualsEither(["Rank"], StringComparison.OrdinalIgnoreCase))
            .SortOrDefault(query)
            .Skip(command.Offset)
            .Take(command.Size);
        return new PaginatedList<WorkspaceStatus>()
        {
            Items = await query.ToListAsync(ct).ConfigureAwait(false),
            TotalCount = totalCount,
        };
    }
}
