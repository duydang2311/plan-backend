using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Issues.GetOne;

public sealed record Response
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueId Id { get; init; }
    public UserId AuthorId { get; init; }
    public TeamId TeamId { get; init; }
    public Team Team { get; init; } = null!;
    public long OrderNumber { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public StatusId? StatusId { get; init; }
    public Status? Status { get; init; }
    public long OrderByStatus { get; init; }
    public IssuePriority Priority { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this Issue issue);
}
