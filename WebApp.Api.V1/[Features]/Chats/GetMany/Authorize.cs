using FastEndpoints;

namespace WebApp.Api.V1.Chats.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!context.Request.UserId.HasValue || context.Request.RequestingUserId != context.Request.UserId.Value)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
