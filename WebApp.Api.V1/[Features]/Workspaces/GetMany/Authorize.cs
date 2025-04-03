using FastEndpoints;

namespace WebApp.Api.V1.Workspaces.GetMany;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return Task.CompletedTask;
        }

        if (!context.Request.UserId.HasValue)
        {
            return context.HttpContext.Response.SendForbiddenAsync(ct);
        }

        return Task.CompletedTask;
    }
}
