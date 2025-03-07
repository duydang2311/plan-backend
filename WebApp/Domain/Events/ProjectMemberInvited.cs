using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ProjectMemberInvited : IEvent
{
    public required ProjectMemberInvitation ProjectMemberInvitation { get; init; }
}
