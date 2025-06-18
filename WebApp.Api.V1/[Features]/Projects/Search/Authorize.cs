using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.Projects.Search;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var hasPermission =
            context.Request.WorkspaceId.HasValue
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    context.Request.WorkspaceId.Value,
                    context.Request.RequestingUserId,
                    Permit.ReadProject,
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
