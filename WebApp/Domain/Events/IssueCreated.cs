using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCreated
{
    public required Issue Issue { get; init; }
    public required UserId AuthorId { get; init; }
}
