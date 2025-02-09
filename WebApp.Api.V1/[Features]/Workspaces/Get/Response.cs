using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Workspaces.Get;

public sealed record Response
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public WorkspaceId? Id { get; init; }
    public string? Name { get; init; } = string.Empty;
    public string? Path { get; init; } = string.Empty;
    public int? TotalProjects { get; init; }
    public int? TotalUsers { get; init; }
    public ICollection<WorkspaceStatus>? Statuses { get; init; }

    public sealed class WorkspaceStatus
    {
        public StatusId? Id { get; init; }
        public int? Rank { get; init; }
        public string? Value { get; init; } = string.Empty;
        public string? Color { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool? IsDefault { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this Workspace workspace);
}
