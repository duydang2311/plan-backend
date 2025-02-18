using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAudits.Delete;

public sealed class Authorize : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var audit = await db
            .IssueAudits.Where(a => a.Id == request.Id && a.UserId == request.RequestingUserId)
            .Select(a => new
            {
                a.Id,
                a.IssueId,
                a.UserId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var isAuthor = audit is not null && audit.Id == request.Id && audit.UserId == request.RequestingUserId;

        return isAuthor
            || (
                audit is not null
                && await db
                    .ProjectMembers.AnyAsync(
                        a =>
                            a.UserId == request.RequestingUserId
                            && a.Project.Issues.Any(b => b.Id == audit.IssueId)
                            && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.DeleteIssueAudit)),
                        ct
                    )
                    .ConfigureAwait(false)
            );
    }
}
