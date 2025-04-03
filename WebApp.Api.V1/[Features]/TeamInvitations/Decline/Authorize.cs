using Ardalis.GuardClauses;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.TeamInvitations.Decline;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return Task.CompletedTask;
        }

        return CheckAsync(context, ct);
        static async Task CheckAsync(IPreProcessorContext<Request> context, CancellationToken ct)
        {
            Guard.Against.Null(context.Request);
            var db = context.HttpContext.Resolve<AppDbContext>();
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
}
