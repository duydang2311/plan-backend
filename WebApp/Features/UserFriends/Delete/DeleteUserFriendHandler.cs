using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriends.Delete;

public sealed record DeleteUserFriendHandler(AppDbContext db)
    : ICommandHandler<DeleteUserFriend, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteUserFriend command, CancellationToken ct)
    {
        Expression<Func<UserFriend, bool>> whereExpression =
            command.UserId.Value > command.FriendId.Value
                ? a => a.UserId == command.FriendId && a.FriendId == command.UserId
                : a => a.UserId == command.UserId && a.FriendId == command.FriendId;

        var count = await db.UserFriends.Where(whereExpression).ExecuteDeleteAsync(ct).ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
