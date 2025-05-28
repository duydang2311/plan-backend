using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseProjectDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public WorkspaceId? WorkspaceId { get; init; }
    public Workspace? Workspace { get; init; }
    public ProjectId? Id { get; init; }
    public string? Name { get; init; }
    public string? Identifier { get; init; }
    public string? Description { get; init; }
    public Instant? DeletedTime { get; init; }
}
