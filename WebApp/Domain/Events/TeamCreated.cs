using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record TeamCreated : IEvent
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required Team Team { get; init; }
    public required UserId UserId { get; init; }
}
