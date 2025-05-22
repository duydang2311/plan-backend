using FastEndpoints;

namespace WebApp.Api.V1.UserProfiles.Patch;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var isCurrentUser = context.Request.UserId == context.Request.RequestingUserId;
        if (!isCurrentUser)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
            return;
        }
    }
}
