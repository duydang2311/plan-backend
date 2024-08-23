using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.TeamInvitations.Accept;

public sealed class Authorize(AppDbContext db) : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var isSameUser = await db
            .TeamInvitations.AnyAsync(
                a => a.Id == context.Request.TeamInvitationId && a.MemberId == context.Request.UserId,
                ct
            )
            .ConfigureAwait(false);
        if (!isSameUser)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
