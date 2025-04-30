using EntityFramework.Exceptions.Common;
using FastEndpoints;
using FractionalIndexing;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.Create;

public sealed record CreateWorkspaceResourceHandler(AppDbContext db)
    : ICommandHandler<CreateWorkspaceResource, OneOf<WorkspaceNotFoundError, UserNotFoundError, Success>>
{
    public async Task<OneOf<WorkspaceNotFoundError, UserNotFoundError, Success>> ExecuteAsync(
        CreateWorkspaceResource command,
        CancellationToken ct
    )
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

        if (command.Files is not null)
        {
            foreach (var file in command.Files)
            {
                var ids = command
                    .Files.Where(a => a.PendingUploadId.HasValue)
                    .Select(a => a.PendingUploadId)
                    .Cast<StoragePendingUploadId>()
                    .ToList();
                await db
                    .StoragePendingUploads.Where(a => ids.Contains(a.Id))
                    .ExecuteDeleteAsync(ct)
                    .ConfigureAwait(false);
            }
        }

        var lastResource = await db
            .WorkspaceResources.Where(a => a.WorkspaceId == command.WorkspaceId)
            .OrderByDescending(a => a.Resource.Rank)
            .Select(a => new { a.Resource.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var previewContent = !string.IsNullOrEmpty(command.Content)
            ? HtmlHelper.ConvertToPlainText(command.Content, 256)
            : null;
        if (previewContent != null && previewContent.Length >= 256)
        {
            previewContent = previewContent[..256];
        }
        var resource = new WorkspaceResource
        {
            WorkspaceId = command.WorkspaceId,
            Resource = new Resource
            {
                CreatorId = command.CreatorId,
                Name = command.Name,
                Document = !string.IsNullOrEmpty(command.Content)
                    ? new ResourceDocument { Content = command.Content, PreviewContent = previewContent }
                    : null,
                Files =
                    command
                        .Files?.Select(a => new ResourceFile
                        {
                            Key = a.Key,
                            OriginalName = a.OriginalName,
                            Size = a.Size,
                            MimeType = a.MimeType,
                        })
                        .ToList() ?? [],
                Rank = OrderKeyGenerator.GenerateKeyBetween(lastResource?.Rank, null),
            },
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
            if (e.ConstraintProperties.Any(a => a == nameof(Resource.CreatorId)))
            {
                return new UserNotFoundError();
            }
            throw;
        }
    }
}
