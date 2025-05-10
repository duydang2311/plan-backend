using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAssignees.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var issue = await db
            .Issues.Where(a => a.Id == context.Request.IssueId)
            .Select(a => new { a.ProjectId, a.Project.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var hasPermission =
            issue is not null
            && (
                await permissionCache
                    .HasProjectPermissionAsync(
                        issue.ProjectId,
                        context.Request.RequestingUserId,
                        Permit.ReadIssueAssignee,
                        ct
                    )
                    .ConfigureAwait(false)
                || await permissionCache
                    .HasWorkspacePermissionAsync(
                        issue.WorkspaceId,
                        context.Request.RequestingUserId,
                        Permit.ReadIssueAssignee,
                        ct
                    )
                    .ConfigureAwait(false)
            );

        if (!hasPermission)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
