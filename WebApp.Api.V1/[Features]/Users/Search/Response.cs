using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.Users.Search;

namespace WebApp.Api.V1.Users.Search;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public UserId UserId { get; init; }
        public required string Email { get; init; }
        public double Similarity { get; init; }
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this SearchUsersResult result);
}
