using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ResourceFiles.CreateBatch;

public sealed class CreateResourceFileBatchHandler(AppDbContext db)
    : ICommandHandler<CreateResourceFileBatch, OneOf<ResourceNotFoundError, IReadOnlyCollection<ResourceFileId>>>
{
    public async Task<OneOf<ResourceNotFoundError, IReadOnlyCollection<ResourceFileId>>> ExecuteAsync(
        CreateResourceFileBatch command,
        CancellationToken ct
    )
    {
        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);

        var pendingUploadIds = command
            .Files.Where(a => a.PendingUploadId.HasValue)
            .Select(a => a.PendingUploadId!.Value)
            .ToList();

        await db
            .StoragePendingUploads.Where(a => pendingUploadIds.Contains(a.Id))
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        var resourceFiles = command
            .Files.Select(file => new ResourceFile
            {
                ResourceId = file.ResourceId,
                Key = file.Key,
                OriginalName = file.OriginalName,
                Size = file.Size,
                MimeType = file.MimeType,
            })
            .ToList();
        db.AddRange(resourceFiles);

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            await transaction.CommitAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            if (
                e.ConstraintProperties.Any(a =>
                    a.Equals(nameof(ResourceFile.ResourceId), StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return new ResourceNotFoundError();
            }
            throw;
        }

        return resourceFiles.Select(rf => rf.Id).ToList();
    }
}
