using FastEndpoints;

namespace WebApp.Api.V1.UserNotifications.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.Request.UserId != context.Request.RequestingUserId)
        {
            return context.HttpContext.Response.SendForbiddenAsync(ct);
        }
        return Task.CompletedTask;
    }
}
