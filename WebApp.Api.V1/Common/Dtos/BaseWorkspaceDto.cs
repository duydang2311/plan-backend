using NodaTime;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseWorkspaceDto
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public WorkspaceId? Id { get; init; }
    public string? Name { get; init; }
    public string? Path { get; init; }
    public int? TotalProjects { get; init; }
    public int? TotalUsers { get; init; }

    // public ICollection<BaseUserDto>? Users { get; init; }
    // public ICollection<WorkspaceStatus>? Statuses { get; init; }
    // public ICollection<WorkspaceFieldDefinition>? FieldDefinitions { get; init; }
    // public ICollection<WorkspaceMember>? Members { get; init; }
    // public ICollection<Project>? Projects { get; init; }
}
