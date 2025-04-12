using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCommentCreated
{
    public required IssueId IssueId { get; init; }
    public required long IssueAuditId { get; init; }
}
