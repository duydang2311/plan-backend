using FastEndpoints;

namespace WebApp.Api.V1.UserFriends.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return Task.CompletedTask;
        }

        if (context.Request.UserId != context.Request.RequestingUserId)
        {
            return context.HttpContext.Response.SendForbiddenAsync(ct);
        }

        return Task.CompletedTask;
    }
}
