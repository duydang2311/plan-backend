using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.GetPermissions.ById;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var workspaceMember = await db
            .WorkspaceMembers.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.WorkspaceId, a.UserId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canRead =
            workspaceMember is not null
            && (
                context.Request.RequestingUserId == workspaceMember.UserId
                || await permissionCache
                    .HasWorkspacePermissionAsync(
                        workspaceMember.WorkspaceId,
                        workspaceMember.UserId,
                        Permit.ReadWorkspaceMember,
                        ct
                    )
                    .ConfigureAwait(false)
            );
        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
