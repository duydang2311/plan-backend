using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.Search;

public sealed record SearchUsersResult : PaginatedList<SearchUsersResult.Item>
{
    public sealed record Item
    {
        public UserId UserId { get; init; }
        public required string Email { get; init; }
        public double Similarity { get; init; }
    }
}
