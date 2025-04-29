using WebApp.Domain.Events;
using WebApp.Infrastructure.Storages.Abstractions;

namespace WebApp.Features.ResourceFiles.Delete;

public static class ResourceFileDeletedHandler
{
    public static async Task HandleAsync(
        ResourceFileDeleted deleted,
        IStorageService storageService,
        CancellationToken ct
    )
    {
        await storageService.DeleteAsync(deleted.Key, ct).ConfigureAwait(false);
    }
}
