using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.Patch;

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
            .Select(a => new { a.WorkspaceId, a.Role.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (workspaceMember is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var hasPermission = await permissionCache
            .HasWorkspacePermissionAsync(
                workspaceMember.WorkspaceId,
                context.Request.RequestingUserId,
                Permit.UpdateWorkspaceMember,
                ct
            )
            .ConfigureAwait(false);
        if (!hasPermission)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var requestingMember = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == workspaceMember.WorkspaceId && a.UserId == context.Request.RequestingUserId
            )
            .Select(a => new { a.Role.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var isLowerRank = requestingMember is null || requestingMember.Rank >= workspaceMember.Rank;
        if (isLowerRank)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
