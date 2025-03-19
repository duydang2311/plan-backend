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

        if (
            (!context.Request.UserId.HasValue && !context.Request.FriendId.HasValue)
            || (context.Request.UserId.HasValue && context.Request.UserId.Value != context.Request.RequestingUserId)
            || (context.Request.FriendId.HasValue && context.Request.FriendId.Value != context.Request.RequestingUserId)
        )
        {
            return context.HttpContext.Response.SendForbiddenAsync(ct);
        }

        return Task.CompletedTask;
    }
}
