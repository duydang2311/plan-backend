using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceInvitations.Accept;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canAccept = await db
            .WorkspaceInvitations.AnyAsync(
                a => a.Id == context.Request.Id && a.UserId == context.Request.RequestingUserId,
                ct
            )
            .ConfigureAwait(false);
        if (!canAccept)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
