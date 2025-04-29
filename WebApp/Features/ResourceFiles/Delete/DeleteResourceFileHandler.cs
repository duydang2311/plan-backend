using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.ResourceFiles.Delete;

public sealed class DeleteResourceFileHandler(AppDbContext db, IDbContextOutbox outbox)
    : ICommandHandler<DeleteResourceFile, OneOf<ServerError, NotFoundError, Success>>
{
    public async Task<OneOf<ServerError, NotFoundError, Success>> ExecuteAsync(
        DeleteResourceFile command,
        CancellationToken ct
    )
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);

        var resourceFile = await db
            .ResourceFiles.Where(a => a.Id == command.Id)
            .Select(a => new { a.Key })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (resourceFile is null)
        {
            return new NotFoundError();
        }

        var count = await db.ResourceFiles.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }

        try
        {
            outbox.Enroll(db);
            await outbox.PublishAsync(new ResourceFileDeleted { Key = resourceFile.Key }).ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch
        {
            return Errors.Outbox();
        }
        return new Success();
    }
}
