using FastEndpoints;
using WebApp.Domain.Events;

namespace WebApp.Features.UserNotifications.Create;

public class ProjectMemberInvitedHandler : IEventHandler<ProjectMemberInvited>
{
    public async Task HandleAsync(ProjectMemberInvited eventModel, CancellationToken ct)
    {
        await new NotifyProjectMemberInvitedJob
        {
            ProjectMemberInvitationId = eventModel.ProjectMemberInvitation.Id,
            MemberId = eventModel.ProjectMemberInvitation.UserId,
        }
            .QueueJobAsync(ct: ct)
            .ConfigureAwait(false);
    }
}
