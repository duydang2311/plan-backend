using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Issues.GetOne;

public sealed record Response
{
    public Instant? CreatedTime { get; init; }
    public Instant? UpdatedTime { get; init; }
    public IssueId? Id { get; init; }
    public UserId? AuthorId { get; init; }
    public User? Author { get; init; }
    public ProjectId? ProjectId { get; init; }
    public Project? Project { get; init; }
    public long? OrderNumber { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public StatusId? StatusId { get; init; }
    public Status? Status { get; init; }
    public string? StatusRank { get; init; }
    public IssuePriority? Priority { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    [MapperIgnoreSource(nameof(Issue.IsDeleted))]
    [MapperIgnoreSource(nameof(Issue.DeletedTime))]
    [MapperIgnoreSource(nameof(Issue.Comments))]
    [MapperIgnoreSource(nameof(Issue.Fields))]
    [MapperIgnoreSource(nameof(Issue.TeamIssues))]
    [MapperIgnoreSource(nameof(Issue.Teams))]
    public static partial Response ToResponse(this Issue issue);
}
