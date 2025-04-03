using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetOne;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canRead = await db
            .ProjectMemberInvitations.AnyAsync(
                a =>
                    a.Id == context.Request.ProjectMemberInvitationId
                    && (
                        a.UserId == context.Request.RequestingUserId
                        || a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadProjectMemberInvitation))
                    ),
                ct
            )
            .ConfigureAwait(false);

        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
