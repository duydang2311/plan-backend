using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Issues.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public IssueId Id { get; init; }
        public UserId AuthorId { get; init; }
        public TeamId TeamId { get; init; }
        public long OrderNumber { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public StatusId? StatusId { get; init; }
        public Status? Status { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<Issue> issue);
}
