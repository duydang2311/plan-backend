using FastEndpoints;

namespace WebApp.Api.V1.TeamInvitations.GetMany.ByMemberId;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }

        if (context.Request.MemberId != context.Request.UserId)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
