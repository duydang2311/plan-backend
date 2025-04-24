using System.Collections;
using System.Text;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Storages.Abstractions;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrls;

public sealed class CreateWorkspaceResourceUploadUrlsHandler(
    AppDbContext db,
    IStorageService storageService,
    ILogger<CreateWorkspaceResourceUploadUrlsHandler> logger
)
    : ICommandHandler<
        CreateWorkspaceResourceUploadUrls,
        OneOf<NotFoundError, ServerError, CreateWorkspaceResourceUploadUrlsResult>
    >
{
    static readonly BitArray isAllowedChar = BuildAllowedCharsLookup(
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_."
    );

    public async Task<OneOf<NotFoundError, ServerError, CreateWorkspaceResourceUploadUrlsResult>> ExecuteAsync(
        CreateWorkspaceResourceUploadUrls command,
        CancellationToken ct
    )
    {
        if (!await db.Workspaces.AnyAsync(x => x.Id == command.WorkspaceId, ct).ConfigureAwait(false))
        {
            return new NotFoundError();
        }

        var list = command
            .Keys.Select(a =>
            {
                var key = Path.Join("ws-resources", command.WorkspaceId.ToBase64String(), SanitizeKey(a));
                return new
                {
                    PendingUpload = new StoragePendingUpload
                    {
                        Key = key,
                        ExpiryTime = SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromHours(1)),
                    },
                    Url = storageService.GeneratePreSignedUploadUrl(key, expiration: TimeSpan.FromHours(30)),
                };
            })
            .ToList();
        db.AddRange(list.Select(a => a.PendingUpload));

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to save pending uploads");
            return ServerError.From("pending_uploads_save_failed");
        }

        return new CreateWorkspaceResourceUploadUrlsResult
        {
            Results =
            [
                .. list.Select(a => new CreateWorkspaceResourceUploadUrlResult
                {
                    PendingUploadId = a.PendingUpload.Id,
                    Url = a.Url,
                }),
            ],
        };
    }

    static string SanitizeKey(string key, string replacement = "_")
    {
        key = key.Trim(' ', '.', '_', '-');

        if (string.IsNullOrEmpty(key))
        {
            return Guid.NewGuid().ToString();
        }

        var sb = new StringBuilder();
        var replacementLastAppended = false;
        foreach (var c in key)
        {
            var idx = (byte)c - 45;
            if (idx < isAllowedChar.Length && isAllowedChar[idx])
            {
                sb.Append(c);
                if (replacementLastAppended)
                {
                    replacementLastAppended = false;
                }
            }
            else if (!replacementLastAppended)
            {
                sb.Append(replacement);
                replacementLastAppended = true;
            }
        }
        var result = sb.ToString();

        result = result.Trim(' ', '.', '_', '-');
        if (string.IsNullOrEmpty(result))
        {
            return Guid.NewGuid().ToString();
        }

        return result;
    }

    static BitArray BuildAllowedCharsLookup(string allowedChars)
    {
        var lookup = new BitArray(78, false);
        foreach (var c in allowedChars)
        {
            if (c < 78)
            {
                lookup[(byte)c - 45] = true;
            }
        }
        return lookup;
    }
}
