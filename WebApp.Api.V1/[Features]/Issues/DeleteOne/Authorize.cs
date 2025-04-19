using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.DeleteOne;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canDelete = await db
            .Issues.AnyAsync(x => x.Id == context.Request.IssueId && x.AuthorId == context.Request.UserId, ct)
            .ConfigureAwait(false);
        if (!canDelete)
        {
            var issue = await db
                .Issues.Where(a => a.Id == context.Request.IssueId)
                .Select(a => new { a.ProjectId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);
            if (issue is not null)
            {
                var projectPermissions = await permissionCache
                    .GetProjectPermissionsAsync(issue.ProjectId, context.Request.UserId, ct)
                    .ConfigureAwait(false);
                canDelete = projectPermissions.Contains(Permit.DeleteIssue);
            }
        }

        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
