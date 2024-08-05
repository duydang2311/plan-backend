using NodaTime;
using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.IssueComments.GetOne;

public sealed record Response
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public IssueCommentId Id { get; init; }
    public User Author { get; init; } = null!;
    public Issue Issue { get; init; } = null!;
    public string Content { get; init; } = string.Empty;
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this IssueComment issueComment);
}
