using FastEndpoints;

namespace WebApp.Api.V1.Common;

public abstract class AuthorizePreProcessor<TRequest> : IPreProcessor<TRequest>
{
    public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        if (!await AuthorizeAsync(context.Request, context, ct).ConfigureAwait(false))
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }

    public abstract Task<bool> AuthorizeAsync(
        TRequest request,
        IPreProcessorContext<TRequest> context,
        CancellationToken ct
    );
}
