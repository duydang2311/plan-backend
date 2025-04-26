using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrls;

public sealed record CreateWorkspaceResourceUploadUrlsResult
{
    public required IReadOnlyCollection<CreateWorkspaceResourceUploadUrlResult> Results { get; init; }
}

public sealed record CreateWorkspaceResourceUploadUrlResult
{
    public required StoragePendingUploadId PendingUploadId { get; init; }
    public required string Key { get; init; }
    public required string Url { get; init; }
}
