using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record WorkspaceCreated : IEvent
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required Workspace Workspace { get; init; }
    public required UserId UserId { get; init; }
}
