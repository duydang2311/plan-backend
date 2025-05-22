using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserSocialLinks.Delete;

public sealed class DeleteUserSocialLinkHandler(AppDbContext db)
    : ICommandHandler<DeleteUserSocialLink, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteUserSocialLink command, CancellationToken ct)
    {
        var count = await db
            .UserSocialLinks.Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
