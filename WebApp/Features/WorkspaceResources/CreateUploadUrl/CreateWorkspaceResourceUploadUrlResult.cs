using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrl;

public sealed record CreateWorkspaceResourceUploadUrlResult
{
    public required StoragePendingUploadId PendingUploadId { get; init; }
    public required string Url { get; init; }
}
