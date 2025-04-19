using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.Issues.GetOne.ByOrderNumber;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var projectPermissions = await permissionCache
            .GetProjectPermissionsAsync(context.Request.ProjectId, context.Request.UserId, ct)
            .ConfigureAwait(false);
        var canRead = projectPermissions.Contains(Permit.ReadIssue);

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
