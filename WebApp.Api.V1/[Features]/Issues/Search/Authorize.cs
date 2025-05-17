using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.Search;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.ProjectId.HasValue)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var project = await db
            .Projects.Where(a => a.Id == context.Request.ProjectId.Value)
            .Select(a => new { a.Id, a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (project is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var hasPermission =
            await permissionCache
                .HasProjectPermissionAsync(project.Id, context.Request.RequestingUserId, Permit.ReadIssue, ct)
                .ConfigureAwait(false)
            || await permissionCache
                .HasWorkspacePermissionAsync(
                    project.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.ReadIssue,
                    ct
                )
                .ConfigureAwait(false);
        if (!hasPermission)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
