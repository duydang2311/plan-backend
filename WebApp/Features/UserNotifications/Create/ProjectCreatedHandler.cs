using FastEndpoints;
using WebApp.Domain.Events;

namespace WebApp.Features.UserNotifications.Create;

public sealed class ProjectCreatedHandler : IEventHandler<ProjectCreated>
{
    public async Task HandleAsync(ProjectCreated eventModel, CancellationToken ct)
    {
        await new NotifyProjectCreatedJob
        {
            ProjectId = eventModel.Project.Id,
            WorkspaceId = eventModel.Project.WorkspaceId,
        }
            .QueueJobAsync(ct: ct)
            .ConfigureAwait(false);
    }
}
