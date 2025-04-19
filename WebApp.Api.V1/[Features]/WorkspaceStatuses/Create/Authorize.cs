using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.WorkspaceStatuses.Create;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var canCreate = await permissionCache
            .HasWorkspacePermissionAsync(
                context.Request.WorkspaceId,
                context.Request.UserId,
                Permit.CreateWorkspaceStatus,
                ct
            )
            .ConfigureAwait(false);
        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
