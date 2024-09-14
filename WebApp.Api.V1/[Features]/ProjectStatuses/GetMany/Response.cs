using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectStatuses.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public StatusId StatusId { get; init; }
        public Status? Status { get; init; }
    }

    public sealed record Status
    {
        public StatusId Id { get; init; }
        public int Rank { get; init; }
        public string Value { get; init; } = string.Empty;
        public string Color { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<ProjectStatus> list);
}
