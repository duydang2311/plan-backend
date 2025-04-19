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
            .Select(a => new { a.ProjectId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (issue is null)
        {
            return false;
        }

        var projectPermissions = await permissionCache
            .GetProjectPermissionsAsync(issue.ProjectId, request.RequestingUserId, ct)
            .ConfigureAwait(false);
        return projectPermissions.Contains(Permit.CreateIssueAuditComment);
    }
}
