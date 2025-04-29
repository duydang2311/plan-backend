using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceResources.Patch;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var workspaceResource = await db
            .WorkspaceResources.Where(a => a.ResourceId == context.Request.Id)
            .Select(a => new { a.WorkspaceId, a.ResourceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var canPatch =
            workspaceResource is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    workspaceResource.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.UpdateWorkspaceResource,
                    ct
                )
                .ConfigureAwait(false);

        if (!canPatch)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
