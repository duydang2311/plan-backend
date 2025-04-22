namespace WebApp.Features.WorkspaceResources.CreateUploadUrl;

public sealed record CreateWorkspaceResourceUploadUrlResult
{
    public required string Url { get; init; }
}
