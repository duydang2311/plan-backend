using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceStatuses.Patch;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var workspaceStatus = await db
            .WorkspaceStatuses.Where(a => a.Id == context.Request.StatusId)
            .Select(a => new { a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canUpdate =
            workspaceStatus is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    workspaceStatus.WorkspaceId,
                    context.Request.UserId,
                    Permit.UpdateWorkspaceStatus,
                    ct
                )
                .ConfigureAwait(false);
        if (!canUpdate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
