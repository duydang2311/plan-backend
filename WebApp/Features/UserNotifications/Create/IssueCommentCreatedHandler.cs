using FastEndpoints;
using WebApp.Domain.Events;

namespace WebApp.Features.UserNotifications.Create;

public sealed class IssueCommentCreatedHandler : IEventHandler<IssueCommentCreated>
{
    public async Task HandleAsync(IssueCommentCreated eventModel, CancellationToken ct)
    {
        await new NotifyIssueCommentCreatedJob
        {
            IssueId = eventModel.IssueAudit.IssueId,
            IssueAuditId = eventModel.IssueAudit.Id,
        }
            .QueueJobAsync(ct: ct)
            .ConfigureAwait(false);
    }
}
