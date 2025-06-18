using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.Verify;

public sealed class VerifyUserHandler(AppDbContext db) : ICommandHandler<VerifyUser, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(VerifyUser command, CancellationToken ct)
    {
        var verification = await db
            .UserVerificationTokens.Where(a => a.Token == command.Token)
            .Select(a => new { a.UserId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (verification is null)
        {
            return new NotFoundError();
        }

        var count = await db
            .UserVerificationTokens.Where(a => a.Token == command.Token)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }

        await db
            .Users.Where(a => a.Id == verification.UserId && !a.IsVerified)
            .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsVerified, true), ct)
            .ConfigureAwait(false);
        return new Success();
    }
}
