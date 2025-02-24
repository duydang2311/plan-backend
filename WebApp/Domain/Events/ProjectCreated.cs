using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ProjectCreated : IEvent
{
    public required Project Project { get; init; }
}
