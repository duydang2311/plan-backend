using FastEndpoints;

namespace WebApp.Api.V1.UserProfiles.Create;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }

        if (context.Request.RequestingUserId != context.Request.UserId)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
