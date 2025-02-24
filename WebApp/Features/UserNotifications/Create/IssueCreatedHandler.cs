using FastEndpoints;
using WebApp.Domain.Events;

namespace WebApp.Features.UserNotifications.Create;

public sealed class IssueCreatedHandler : IEventHandler<IssueCreated>
{
    public async Task HandleAsync(IssueCreated eventModel, CancellationToken ct)
    {
        await new NotifyIssueCreatedJob { IssueId = eventModel.Issue.Id, ProjectId = eventModel.Issue.ProjectId }
            .QueueJobAsync(ct: ct)
            .ConfigureAwait(false);
    }
}
