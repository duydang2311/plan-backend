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
            .Issues.AnyAsync(x => x.Id == context.Request.IssueId && x.AuthorId == context.Request.UserId, ct)
            .ConfigureAwait(false);
        if (!canUpdate)
        {
            var issue = await dbContext
                .Issues.Where(x => x.Id == context.Request.IssueId)
                .Select(x => new { x.ProjectId })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);
            if (issue is not null)
            {
                var projectPermissions = await permissionCache
                    .GetProjectPermissionsAsync(issue.ProjectId, context.Request.UserId, ct)
                    .ConfigureAwait(false);
                canUpdate = projectPermissions.Contains(Permit.UpdateIssue);
            }
        }
        if (!canUpdate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
