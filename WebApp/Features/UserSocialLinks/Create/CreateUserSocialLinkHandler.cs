using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserSocialLinks.Create;

public sealed class CreateUserSocialLinkHandler(AppDbContext db)
    : ICommandHandler<CreateUserSocialLink, OneOf<NotFoundError, UserSocialLink>>
{
    public async Task<OneOf<NotFoundError, UserSocialLink>> ExecuteAsync(
        CreateUserSocialLink command,
        CancellationToken ct
    )
    {
        var userSocialLink = new UserSocialLink { UserId = command.UserId, Url = command.Url };

        db.Add(userSocialLink);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            if (
                e.ConstraintProperties.Any(a =>
                    a.Equals(nameof(UserSocialLink.UserId), StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return new NotFoundError();
            }
            throw;
        }

        return userSocialLink;
    }
}
