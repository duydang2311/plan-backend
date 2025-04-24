using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.CreateUploadUrls;

namespace WebApp.Api.V1.WorkspaceResources.CreateUploadUrl;

public sealed record Response
{
    public required string Url { get; init; }
    public required StoragePendingUploadId PendingUploadId { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static Response ToResponse(this CreateWorkspaceResourceUploadUrlsResult result)
    {
        var first = result.Results.First();
        return new() { Url = first.Url, PendingUploadId = first.PendingUploadId };
    }
}
