using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.IssueComments.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public Instant CreatedTime { get; init; }
        public Instant UpdatedTime { get; init; }
        public IssueCommentId Id { get; init; }
        public User? Author { get; init; }
        public Issue? Issue { get; init; }
        public string Content { get; init; } = string.Empty;
    }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<IssueComment> list);
}
