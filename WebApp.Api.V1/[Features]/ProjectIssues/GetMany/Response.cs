using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ProjectIssues.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public IssueId IssueId { get; init; }
        public Issue? Issue { get; init; }
        public string Rank { get; init; } = string.Empty;
    }

    public sealed record Issue
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public IssueId Id { get; init; }
        public UserId AuthorId { get; init; }
        public User? Author { get; init; }
        public TeamId TeamId { get; init; }
        public Team? Team { get; init; }
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
    public static partial Response ToReponse(this PaginatedList<ProjectIssue> list);
}
