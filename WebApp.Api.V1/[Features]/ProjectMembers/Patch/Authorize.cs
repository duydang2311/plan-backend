using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMembers.Patch;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var projectMember = await db
            .ProjectMembers.Where(a => a.Id == context.Request.ProjectMemberId)
            .Select(a => new
            {
                a.ProjectId,
                RoleId = a.Role.Id,
                a.Role.Rank,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (projectMember is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var canPatch = await permissionCache
            .HasProjectPermissionAsync(
                projectMember.ProjectId,
                context.Request.RequestingUserId,
                Permit.UpdateProjectMember,
                ct
            )
            .ConfigureAwait(false);
        if (!canPatch)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var requestingProjectMember = await db
            .ProjectMembers.Where(a => a.UserId == context.Request.RequestingUserId)
            .Select(a => new { a.Role.Rank })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (requestingProjectMember is null)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        var hasLowerRank = requestingProjectMember.Rank >= projectMember.Rank;
        if (hasLowerRank)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
