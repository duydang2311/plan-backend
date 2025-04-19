using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetOne.ById;

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
            .Select(a => new { a.ProjectId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (issue is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var projectPermissions = await permissionCache
            .GetProjectPermissionsAsync(issue.ProjectId, context.Request.UserId, ct)
            .ConfigureAwait(false);
        var canRead = projectPermissions.Contains(Permit.ReadIssue);

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
