using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceInvitations.Delete;

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
            .Select(a => new { a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canDelete =
            workspaceInvitation is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    workspaceInvitation.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.CreateWorkspaceInvitation,
                    ct
                )
                .ConfigureAwait(false);
        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
