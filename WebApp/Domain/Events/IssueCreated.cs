using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCreated
{
    public required IssueId IssueId { get; init; }
    public required ProjectId ProjectId { get; init; }
}
