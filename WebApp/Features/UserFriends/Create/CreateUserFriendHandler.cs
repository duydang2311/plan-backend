using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriends.Create;

public sealed record CreateUserFriendHandler(AppDbContext db)
    : ICommandHandler<CreateUserFriend, OneOf<ValidationFailures, DuplicatedError, Success>>
{
    public async Task<OneOf<ValidationFailures, DuplicatedError, Success>> ExecuteAsync(
        CreateUserFriend command,
        CancellationToken ct
    )
    {
        var userFriend = new UserFriend { UserId = command.UserId, FriendId = command.FriendId };

        db.Add(userFriend);
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
                    nameof(UserFriendRequest.ReceiverId) => ("receiverId", "Receiver does not exist"),
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
