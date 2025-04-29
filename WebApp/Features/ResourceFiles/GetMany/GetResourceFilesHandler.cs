using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ResourceFiles.GetMany;

public sealed class GetResourceFilesHandler(AppDbContext db)
    : ICommandHandler<GetResourceFiles, PaginatedList<ResourceFile>>
{
    public async Task<PaginatedList<ResourceFile>> ExecuteAsync(GetResourceFiles command, CancellationToken ct)
    {
        var query = db.ResourceFiles.Where(a => a.ResourceId == command.ResourceId);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (command.Cursor.HasValue)
        {
            query = query.Where(a => a.Id < command.Cursor.Value);
        }

        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.Id));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ResourceFile, ResourceFile>(command.Select));
        }

        return PaginatedList.From(await query.Take(command.Size).ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
