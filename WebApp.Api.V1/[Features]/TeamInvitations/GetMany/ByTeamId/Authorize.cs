using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.TeamInvitations.GetMany.ByTeamId;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var isInTeam = await db
            .TeamMembers.AnyAsync(a => a.TeamId == context.Request.TeamId && a.MemberId == context.Request.UserId, ct)
            .ConfigureAwait(false);
        if (!isInTeam)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}