using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.Get;

using Result = OneOf<PaginatedList<WorkspaceMember>>;

public sealed class GetWorkspaceMembersHandler(AppDbContext db) : ICommandHandler<GetWorkspaceMembers, Result>
{
    public async Task<Result> ExecuteAsync(GetWorkspaceMembers command, CancellationToken ct)
    {
        var query = db.WorkspaceMembers.Where(a => a.WorkspaceId == command.WorkspaceId).AsQueryable();
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceMember, WorkspaceMember>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command
            .Order.Where(static x =>
                x.Name.EqualsEither(["CreatedTime", "UpdatedTime"], StringComparison.OrdinalIgnoreCase)
            )
            .SortOrDefault(query.OrderByDescending(a => a.CreatedTime));
        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        return new PaginatedList<WorkspaceMember>() { Items = items, TotalCount = totalCount };
    }
}
