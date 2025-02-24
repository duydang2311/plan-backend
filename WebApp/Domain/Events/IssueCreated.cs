using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record IssueCreated : IEvent
{
    public required Issue Issue { get; init; }
}
