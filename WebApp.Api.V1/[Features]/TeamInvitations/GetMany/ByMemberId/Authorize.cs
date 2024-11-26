using Ardalis.GuardClauses;
using FastEndpoints;

namespace WebApp.Api.V1.TeamInvitations.GetMany.ByMemberId;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return Task.CompletedTask;
        }

        return CheckAsync(context, ct);
        static async Task CheckAsync(IPreProcessorContext<Request> context, CancellationToken ct)
        {
            Guard.Against.Null(context.Request);
            if (context.Request.MemberId != context.Request.UserId)
            {
                await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
                return;
            }
        }
    }
}
