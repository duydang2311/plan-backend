using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public record BaseWorkspaceResourceDto
{
    public ResourceId? ResourceId { get; init; }
    public BaseResourceDto? Resource { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public BaseWorkspaceDto? Workspace { get; init; }
}
