using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.WorkspaceResources.CreateToken;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var canRead =
            context.Request.WorkspaceId.HasValue
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    context.Request.WorkspaceId.Value,
                    context.Request.UserId,
                    Permit.ReadWorkspaceResource,
                    ct
                )
                .ConfigureAwait(false);
        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
