using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.Workspaces.SearchItems;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var workspacePermissions = await permissionCache
            .GetWorkspacePermissionsAsync(context.Request.WorkspaceId, context.Request.RequestingUserId, ct)
            .ConfigureAwait(false);
        var hasPermission =
            workspacePermissions.Contains(Permit.ReadProject) && workspacePermissions.Contains(Permit.ReadIssue);
        if (!hasPermission)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
