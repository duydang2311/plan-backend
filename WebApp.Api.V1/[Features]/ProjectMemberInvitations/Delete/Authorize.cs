using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.Delete;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }
        var db = context.HttpContext.Resolve<AppDbContext>();
        var data = await db
            .ProjectMemberInvitations.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.ProjectId, a.Project.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (data is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
        var canDelete =
            await permissionCache
                .HasProjectPermissionAsync(
                    data.ProjectId,
                    context.Request.RequestingUserId,
                    Permit.DeleteProjectMemberInvitation,
                    ct
                )
                .ConfigureAwait(false)
            || await permissionCache
                .HasWorkspacePermissionAsync(
                    data.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.DeleteProjectMemberInvitation,
                    ct
                )
                .ConfigureAwait(false);
        if (!canDelete)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
