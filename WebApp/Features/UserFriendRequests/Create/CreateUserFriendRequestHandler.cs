using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriendRequests.Create;

public sealed record CreateUserFriendRequestHandler(AppDbContext db)
    : ICommandHandler<CreateUserFriendRequest, OneOf<ValidationFailures, DuplicatedError, Success>>
{
    public async Task<OneOf<ValidationFailures, DuplicatedError, Success>> ExecuteAsync(
        CreateUserFriendRequest command,
        CancellationToken ct
    )
    {
        if (
            await db
                .UserFriendRequests.AnyAsync(
                    x => x.SenderId == command.ReceiverId && x.ReceiverId == command.SenderId,
                    ct
                )
                .ConfigureAwait(false)
        )
        {
            return new DuplicatedError();
        }

        var userFriendRequest = new UserFriendRequest { SenderId = command.SenderId, ReceiverId = command.ReceiverId };

        db.Add(userFriendRequest);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(UserFriendRequest.SenderId) => ("senderId", "Sender does not exist"),
                    nameof(UserFriendRequest.ReceiverId) => ("senderId", "Receiver does not exist"),
                    _ => null,
                }
            );
        }
        catch (UniqueConstraintException)
        {
            return new DuplicatedError();
        }

        return new Success();
    }
}
