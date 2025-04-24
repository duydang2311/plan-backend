using Riok.Mapperly.Abstractions;
using WebApp.Features.WorkspaceResources.CreateUploadUrls;

namespace WebApp.Api.V1.WorkspaceResources.CreateBatchUploadUrls;

public sealed record Response
{
    public required IReadOnlyCollection<CreateWorkspaceResourceUploadUrlResult> Results { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this CreateWorkspaceResourceUploadUrlsResult result);
}
