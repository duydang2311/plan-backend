using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetOne;

public sealed class Authorize(IPermissionCache permissionCache) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var invitation = await db
            .ProjectMemberInvitations.Where(a => a.Id == context.Request.ProjectMemberInvitationId)
            .Select(a => new { a.UserId, a.ProjectId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canRead =
            invitation is not null
            && (
                invitation.UserId == context.Request.RequestingUserId
                || await permissionCache
                    .HasProjectPermissionAsync(
                        invitation.ProjectId,
                        context.Request.RequestingUserId,
                        Permit.ReadProjectMemberInvitation,
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
