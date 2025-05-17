using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Issues.Search;

public sealed record IssuesResult : PaginatedList<IssuesResult.Item>
{
    public sealed record Item
    {
        public IssueId IssueId { get; init; }
        public required string Email { get; init; }
        public double Similarity { get; init; }
    }
}
