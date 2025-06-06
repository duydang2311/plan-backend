using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceInvitations.GetOne;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var workspaceInvitation = await db
            .WorkspaceInvitations.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.WorkspaceId, a.UserId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canRead =
            workspaceInvitation is not null
            && (
                workspaceInvitation.UserId == context.Request.RequestingUserId
                || await permissionCache
                    .HasWorkspacePermissionAsync(
                        workspaceInvitation.WorkspaceId,
                        context.Request.RequestingUserId,
                        Permit.ReadWorkspaceInvitation,
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
