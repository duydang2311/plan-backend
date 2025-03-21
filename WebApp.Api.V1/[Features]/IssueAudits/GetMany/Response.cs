using System.Text.Json;
using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.IssueAudits.GetMany;

public sealed record Response : PaginatedList<Response.Item>
{
    public sealed record Item
    {
        public long? Id { get; init; }
        public Instant? CreatedTime { get; init; }
        public IssueId? IssueId { get; init; }
        public IssueAuditAction? Action { get; init; }
        public UserId? UserId { get; init; }
        public User? User { get; init; }
        public JsonDocument? Data { get; init; }
    }

    public sealed record User
    {
        public UserId? Id { get; init; }
        public string? Email { get; init; }
        public UserProfile? Profile { get; init; }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this PaginatedList<IssueAudit> list);

    public static partial Response.Item MapResponseItem(this IssueAudit list);

    public static partial Response.User? MapNullableUser(this User? user);

    public static Response.User? MapUser(this User user) => MapNullableUser(user);
}
