using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.GetMany;

public sealed class GetUsersHandler(AppDbContext db) : ICommandHandler<GetUsers, OneOf<PaginatedList<User>>>
{
    public async Task<OneOf<PaginatedList<User>>> ExecuteAsync(GetUsers command, CancellationToken ct)
    {
        var query = db.Users.AsQueryable();
        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.Workspaces.Any(a => a.Id == command.WorkspaceId.Value));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<User, User>(command.Select));
        }

        query = command
            .Order.Where(a => a.Name.EqualsEither(["CreatedTime"], StringComparison.OrdinalIgnoreCase))
            .SortOrDefault(query, query => query.OrderByDescending(a => a.CreatedTime));

        return PaginatedList.From(await query.ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
