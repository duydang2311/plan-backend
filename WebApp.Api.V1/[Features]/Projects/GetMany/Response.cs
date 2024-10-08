using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Projects.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public WorkspaceId WorkspaceId { get; init; }
        public Workspace? Workspace { get; init; }
        public ProjectId Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Identifier { get; init; } = string.Empty;
        public string? Description { get; init; }
        public ICollection<Status>? Statuses { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Project> list);
}
