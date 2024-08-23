using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record TeamInvitationAccepted : IEvent
{
    public required IServiceProvider ServiceProvider { get; init; }
    public required TeamInvitation TeamInvitation { get; init; }
}
