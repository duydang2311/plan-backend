using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.CreateFile;

public sealed record CreateWorkspaceFileResourceHandler(AppDbContext db)
    : ICommandHandler<
        CreateWorkspaceFileResource,
        OneOf<WorkspaceNotFoundError, UserNotFoundError, InvalidResourceTypeError, Success>
    >
{
    public async Task<OneOf<WorkspaceNotFoundError, UserNotFoundError, InvalidResourceTypeError, Success>> ExecuteAsync(
        CreateWorkspaceFileResource command,
        CancellationToken ct
    )
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

        if (command.PendingUploadId.HasValue)
        {
            await db
                .StoragePendingUploads.Where(a => a.Id == command.PendingUploadId.Value)
                .ExecuteDeleteAsync(ct)
                .ConfigureAwait(false);
        }

        var resource = new WorkspaceResource
        {
            WorkspaceId = command.WorkspaceId,
            Resource = new FileResource { CreatorId = command.CreatorId, Key = command.Key },
        };

        db.Add(resource);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            await transaction.CommitAsync(ct).ConfigureAwait(false);
            return new Success();
        }
        catch (ReferenceConstraintException e)
        {
            if (e.ConstraintProperties.Any(a => a == nameof(WorkspaceResource.WorkspaceId)))
            {
                return new WorkspaceNotFoundError();
            }
            if (e.ConstraintProperties.Any(a => a == nameof(FileResource.CreatorId)))
            {
                return new UserNotFoundError();
            }
            throw;
        }
    }
}
