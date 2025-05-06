using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceMembers.Delete;

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
            .Select(a => new
            {
                a.WorkspaceId,
                a.RoleId,
                a.Role.Rank,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var hasPermission =
            workspaceMember is not null
            && await permissionCache
                .HasWorkspacePermissionAsync(
                    workspaceMember.WorkspaceId,
                    context.Request.RequestingUserId,
                    Permit.DeleteWorkspaceMember,
                    ct
                )
                .ConfigureAwait(false);
        if (!hasPermission)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var isOwner = workspaceMember!.RoleId == WorkspaceRoleDefaults.Owner.Id;
        if (isOwner)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var requestingWorkspaceMember = await db
            .WorkspaceMembers.Where(a =>
                a.WorkspaceId == workspaceMember!.WorkspaceId && a.UserId == context.Request.RequestingUserId
            )
            .Select(a => new { a.Role.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var hasHigherRank =
            requestingWorkspaceMember is not null && requestingWorkspaceMember.Rank < workspaceMember!.Rank;
        if (!hasHigherRank)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
