using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriendRequests.GetMany;

public sealed record GetUserFriendRequestsHandler(AppDbContext db)
    : ICommandHandler<GetUserFriendRequests, PaginatedList<UserFriendRequest>>
{
    public async Task<PaginatedList<UserFriendRequest>> ExecuteAsync(
        GetUserFriendRequests command,
        CancellationToken ct
    )
    {
        if (!command.SenderId.HasValue && !command.ReceiverId.HasValue)
        {
            return PaginatedList.From(Array.Empty<UserFriendRequest>(), 0);
        }

        var query = db.UserFriendRequests.AsQueryable();
        if (command.SenderId.HasValue)
        {
            query = query.Where(x => x.SenderId == command.SenderId.Value);
        }
        if (command.ReceiverId.HasValue)
        {
            query = query.Where(x => x.SenderId == command.ReceiverId.Value);
        }

        var countTask = query.CountAsync(ct);
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<UserFriendRequest, UserFriendRequest>(command.Select));
        }
        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        var totalCount = await countTask.ConfigureAwait(false);
        return PaginatedList.From(await query.ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
