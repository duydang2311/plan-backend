using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.Patch;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        var canUpdate = await dbContext
            .Issues.AnyAsync(a => a.Id == context.Request.IssueId && a.AuthorId == context.Request.UserId, ct)
            .ConfigureAwait(false);
        if (!canUpdate)
        {
            var issue = await dbContext
                .Issues.Where(a => a.Id == context.Request.IssueId)
                .Select(a => new { a.Project.WorkspaceId, a.ProjectId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);
            if (issue is not null)
            {
                canUpdate =
                    await permissionCache
                        .HasWorkspacePermissionAsync(issue.WorkspaceId, context.Request.UserId, Permit.UpdateIssue, ct)
                        .ConfigureAwait(false)
                    || await permissionCache
                        .HasProjectPermissionAsync(issue.ProjectId, context.Request.UserId, Permit.UpdateIssue, ct)
                        .ConfigureAwait(false);
                if (!canUpdate)
                {
                    canUpdate = await dbContext
                        .IssueAssignees.AnyAsync(
                            a => a.IssueId == context.Request.IssueId && a.UserId == context.Request.UserId,
                            ct
                        )
                        .ConfigureAwait(false);
                }
            }
        }
        if (!canUpdate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
