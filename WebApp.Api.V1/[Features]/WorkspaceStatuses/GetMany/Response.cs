using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.WorkspaceStatuses.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public StatusId Id { get; init; }
        public WorkspaceId WorkspaceId { get; init; }
        public int Rank { get; init; }
        public string Value { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        public string? Icon { get; init; }
        public string? Description { get; init; }
        public bool IsDefault { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<WorkspaceStatus> list);
}
