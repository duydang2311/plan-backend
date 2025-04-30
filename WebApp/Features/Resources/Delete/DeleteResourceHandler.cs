using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.Resources.Delete;

public sealed record DeleteResourceHandler(
    AppDbContext db,
    IDbContextOutbox outbox,
    ILogger<DeleteResourceHandler> logger
) : ICommandHandler<DeleteResource, OneOf<ServerError, NotFoundError, Success>>
{
    public async Task<OneOf<ServerError, NotFoundError, Success>> ExecuteAsync(
        DeleteResource command,
        CancellationToken ct
    )
    {
        var keys = await db
            .ResourceFiles.Where(a => a.ResourceId == command.Id)
            .Select(a => a.Key)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        if (keys is null)
        {
            return new NotFoundError();
        }

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        var count = await db.Resources.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);

        outbox.Enroll(db);
        try
        {
            foreach (var key in keys)
            {
                await outbox.PublishAsync(new ResourceFileDeleted { Key = key }).ConfigureAwait(false);
            }
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error publishing resource file deleted event");
            return Errors.Outbox();
        }

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
