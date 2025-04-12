using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ProjectMemberInvited
{
    public required ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
    public required UserId MemberId { get; init; }
}
