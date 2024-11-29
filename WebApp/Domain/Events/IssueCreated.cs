using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCreated : IEvent
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required Issue Issue { get; init; }
    public required UserId AuthorId { get; init; }
    public required ProjectId ProjectId { get; init; }
}
