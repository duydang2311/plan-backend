using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Milestones.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var hasPermission =
            context.Request.ProjectId.HasValue
            && await permissionCache
                .HasProjectPermissionAsync(
                    context.Request.ProjectId.Value,
                    context.Request.RequestingUserId,
                    Permit.ReadMilestone,
                    ct
                )
                .ConfigureAwait(false);
        if (hasPermission)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var project = await db
            .Projects.Where(a => a.Id == context.Request.ProjectId)
            .Select(a => new { a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (project is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        hasPermission = await permissionCache
            .HasWorkspacePermissionAsync(
                project.WorkspaceId,
                context.Request.RequestingUserId,
                Permit.ReadMilestone,
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
