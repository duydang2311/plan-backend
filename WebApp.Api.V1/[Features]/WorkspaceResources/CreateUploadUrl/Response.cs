using Riok.Mapperly.Abstractions;
using WebApp.Features.WorkspaceResources.CreateUploadUrl;

namespace WebApp.Api.V1.WorkspaceResources.CreateUploadUrl;

public sealed record Response
{
    public required string Url { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this CreateWorkspaceResourceUploadUrlResult result);
}
