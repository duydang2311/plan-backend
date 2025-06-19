using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAudits.CreateComment;

public sealed class Authorize(IPermissionCache permissionCache) : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var issue = await db
            .Issues.Where(a => a.Id == request.IssueId)
            .Select(a => new { a.ProjectId, a.Project.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (issue is null)
        {
            return false;
        }

        var canCreate = await permissionCache
            .HasProjectPermissionAsync(issue.ProjectId, request.RequestingUserId, Permit.CreateIssueAuditComment, ct)
            .ConfigureAwait(false);
        if (!canCreate)
        {
            canCreate = await permissionCache
                .HasWorkspacePermissionAsync(
                    issue.WorkspaceId,
                    request.RequestingUserId,
                    Permit.CreateIssueAuditComment,
                    ct
                )
                .ConfigureAwait(false);
        }
        return canCreate;
    }
}
