using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserNotifications.Create;

public sealed class CreateUserNotificationHandler(AppDbContext db)
    : ICommandHandler<CreateUserNotification, OneOf<InvalidUserError, Success>>
{
    public async Task<OneOf<InvalidUserError, Success>> ExecuteAsync(
        CreateUserNotification command,
        CancellationToken ct
    )
    {
        db.Add(
            new UserNotification
            {
                UserId = command.UserId,
                Notification = new Notification { Type = command.Type, Data = command.Data },
            }
        );

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException)
        {
            return new InvalidUserError();
        }

        return new Success();
    }
}
