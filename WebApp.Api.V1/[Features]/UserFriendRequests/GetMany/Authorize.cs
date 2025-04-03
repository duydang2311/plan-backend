using FastEndpoints;

namespace WebApp.Api.V1.UserFriendRequests.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.SenderId.HasValue && !context.Request.ReceiverId.HasValue)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }

        if (
            (context.Request.SenderId.HasValue && context.Request.SenderId.Value != context.Request.RequestingUserId)
            || context.Request.ReceiverId.HasValue
                && context.Request.ReceiverId.Value != context.Request.RequestingUserId
        )
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
