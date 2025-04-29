using FastEndpoints;
using OneOf;
using WebApp.Domain.Entities;

namespace WebApp.Features.ResourceFiles.CreateBatch;

public sealed record CreateResourceFileBatch
    : ICommand<OneOf<ResourceNotFoundError, IReadOnlyCollection<ResourceFileId>>>
{
    public required IReadOnlyCollection<CreateResourceFileBatchItem> Files { get; init; }
}

public sealed record CreateResourceFileBatchItem
{
    public required ResourceId ResourceId { get; init; }
    public required string Key { get; init; }
    public required string OriginalName { get; init; }
    public required long Size { get; init; }
    public required string MimeType { get; init; }
    public StoragePendingUploadId? PendingUploadId { get; init; }
}
