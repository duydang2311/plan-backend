using WebApp.Domain.Entities;

namespace WebApp.Domain.Events;

public sealed record ProjectMemberInvitedUserNotified : UserNotified
{
    public required ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
    public required string ProjectIdentifier { get; init; }
    public required string ProjectName { get; init; }
}
