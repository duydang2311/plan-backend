using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseWorkspaceStatusDto
{
    public StatusId? Id { get; init; }
    public StatusCategory? Category { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public int? Rank { get; init; }
    public string? Value { get; init; }
    public string? Color { get; init; }
    public string? Icon { get; init; }
    public string? Description { get; init; }
    public bool? IsDefault { get; init; }
    public BaseWorkspaceDto? Workspace { get; init; }
}
