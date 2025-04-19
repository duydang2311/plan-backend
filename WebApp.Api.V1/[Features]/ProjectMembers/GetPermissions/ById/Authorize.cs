using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMembers.GetPermissions.ById;

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
            .ProjectMembers.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.ProjectId, a.UserId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canRead =
            projectMember is not null
            && (
                context.Request.RequestingUserId == projectMember.UserId
                || await permissionCache
                    .HasProjectPermissionAsync(
                        projectMember.ProjectId,
                        projectMember.UserId,
                        Permit.ReadProjectMember,
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
