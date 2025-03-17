using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriendRequests.Delete;

public sealed record DeleteUserFriendRequestHandler(AppDbContext db)
    : ICommandHandler<DeleteUserFriendRequest, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteUserFriendRequest command, CancellationToken ct)
    {
        var count = await db
            .UserFriendRequests.Where(a => a.SenderId == command.SenderId && a.ReceiverId == command.ReceiverId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
