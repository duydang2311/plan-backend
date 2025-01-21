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
    public ResponseAuthor? Author { get; init; }
    public ProjectId? ProjectId { get; init; }
    public Project? Project { get; init; }
    public long? OrderNumber { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public StatusId? StatusId { get; init; }
    public Status? Status { get; init; }
    public string? StatusRank { get; init; }
    public IssuePriority? Priority { get; init; }
    public ICollection<Team>? Teams { get; init; } = null!;
    public ICollection<User>? Assignees { get; init; } = null!;
}

public sealed record ResponseAuthor
{
    public UserId? Id { get; init; }
    public string? Email { get; init; }
    public UserProfile? Profile { get; init; } = null!;
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this Issue issue);

    public static partial ResponseAuthor? NullableResponseAuthor(User? user);

    public static ResponseAuthor? ToResponseAuthor(this User user) => NullableResponseAuthor(user);
}
