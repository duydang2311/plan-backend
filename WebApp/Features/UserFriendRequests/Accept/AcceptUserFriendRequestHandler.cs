using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriendRequests.Accept;

public sealed record AcceptUserFriendRequestHandler(AppDbContext db)
    : ICommandHandler<AcceptUserFriendRequest, OneOf<NotFoundError, ConflictError, Success>>
{
    public async Task<OneOf<NotFoundError, ConflictError, Success>> ExecuteAsync(
        AcceptUserFriendRequest command,
        CancellationToken ct
    )
    {
        var count = await db
            .UserFriendRequests.Where(x => x.SenderId == command.SenderId && x.ReceiverId == command.ReceiverId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        db.Add(new UserFriend { UserId = command.SenderId, FriendId = command.ReceiverId });
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (UniqueConstraintException)
        {
            return new ConflictError();
        }

        return new Success();
    }
}
