using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.TeamInvitations.Create;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var canCreate = await db
            .TeamMembers.AnyAsync(
                a =>
                    a.TeamId == context.Request.TeamId
                    && a.MemberId == context.Request.UserId
                    && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.CreateTeamMember)),
                ct
            )
            .ConfigureAwait(false);
        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
