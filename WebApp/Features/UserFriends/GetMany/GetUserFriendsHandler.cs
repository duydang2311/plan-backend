using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriends.GetMany;

public sealed record GetUserFriendsHandler(AppDbContext db) : ICommandHandler<GetUserFriends, PaginatedList<UserFriend>>
{
    public async Task<PaginatedList<UserFriend>> ExecuteAsync(GetUserFriends command, CancellationToken ct)
    {
        var query = db.UserFriends.AsQueryable();

        if (command.UserId.HasValue && command.FriendId.HasValue)
        {
            query = query.Where(a => a.UserId == command.UserId.Value || a.FriendId == command.FriendId.Value);
        }
        else
        {
            if (command.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == command.UserId.Value);
            }

            if (command.FriendId.HasValue)
            {
                query = query.Where(a => a.FriendId == command.FriendId.Value);
            }
        }

        var countTask = query.CountAsync(ct);
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<UserFriend, UserFriend>(command.Select));
        }
        query = command.Order.SortOrDefault(query, a => a.OrderByDescending(b => b.CreatedTime));

        var totalCount = await countTask.ConfigureAwait(false);
        return PaginatedList.From(await query.ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
