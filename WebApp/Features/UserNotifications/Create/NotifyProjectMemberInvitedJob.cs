using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.Create;

public sealed record NotifyProjectMemberInvitedJob : ICommand
{
    public required ProjectMemberInvitationId ProjectMemberInvitationId { get; init; }
    public required UserId MemberId { get; init; }
}
