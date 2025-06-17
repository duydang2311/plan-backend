using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueStatusUpdated
{
    public required IssueId IssueId { get; init; }
    public required ProjectId ProjectId { get; init; }
    public required StatusId? OldStatusId { get; init; }
    public required StatusId? NewStatusId { get; init; }
}
