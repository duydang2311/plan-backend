using System.Collections;
using System.Security.Cryptography;
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

        var uploads = command
            .Keys.Select(a =>
            {
                var key = string.Join('/', ["ws-resources", command.WorkspaceId.ToBase64String(), SanitizeKey(a)]);
                return new StoragePendingUpload
                {
                    Key = key,
                    ExpiryTime = SystemClock.Instance.GetCurrentInstant().Plus(Duration.FromHours(1)),
                };
            })
            .ToList();
        db.AddRange(uploads);

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
                .. await Task.WhenAll(
                        uploads.Select(async a =>
                        {
                            return new CreateWorkspaceResourceUploadUrlResult
                            {
                                PendingUploadId = a.Id,
                                Key = a.Key,
                                Url = await storageService
                                    .GetPreSignedUploadUrlAsync(a.Key, expiration: TimeSpan.FromHours(30))
                                    .ConfigureAwait(false),
                            };
                        })
                    )
                    .ConfigureAwait(false),
            ],
        };
    }

    static string SanitizeKey(string key, string replacement = "_")
    {
        key = key.Trim(' ', '.', '_', '-');

        if (string.IsNullOrEmpty(key))
        {
            return GenerateRandomId();
        }

        var sb = new StringBuilder();
        var replaced = false;
        var extensionStart = -1;
        foreach (var c in key)
        {
            var idx = (byte)c - 45;
            if (idx >= 0 && idx < isAllowedChar.Length && isAllowedChar[idx])
            {
                if (c == '.')
                {
                    extensionStart = sb.Length;
                }
                sb.Append(c);
                if (replaced)
                {
                    replaced = false;
                }
            }
            else if (!replaced)
            {
                sb.Append(replacement);
                replaced = true;
            }
        }

        if (extensionStart == -1)
        {
            if (sb[^1] != '_')
            {
                sb.Append('_');
            }
            sb.Append(GenerateRandomId());
        }
        else
        {
            if (sb[extensionStart - 1] != '_')
            {
                sb.Insert(extensionStart, '_');
                ++extensionStart;
            }
            sb.Insert(extensionStart, GenerateRandomId());
        }
        var result = sb.ToString();

        result = result.Trim(' ', '.', '_', '-');
        if (string.IsNullOrEmpty(result))
        {
            return GenerateRandomId();
        }

        return result;
    }

    static string GenerateRandomId()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    static BitArray BuildAllowedCharsLookup(string allowedChars)
    {
        var lookup = new BitArray(78, false);
        foreach (var c in allowedChars)
        {
            lookup[(byte)c - 45] = true;
        }
        return lookup;
    }
}
