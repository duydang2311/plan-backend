using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.Accept;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canAccept = await db
            .ProjectMemberInvitations.AnyAsync(
                a => a.Id == context.Request.ProjectMemberInvitationId && a.UserId == context.Request.RequestingUserId,
                ct
            )
            .ConfigureAwait(false);
        if (!canAccept)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
