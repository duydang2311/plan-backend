using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCommentCreated
{
    public required IssueComment IssueComment { get; init; }
}
