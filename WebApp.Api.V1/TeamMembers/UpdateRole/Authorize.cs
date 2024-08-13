using FastEndpoints;

namespace WebApp.Api.V1.TeamMembers.UpdateRole;

public sealed class Authorize : IPreProcessor<Request>
{
    public Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        // TODO: authorize
        return Task.CompletedTask;
    }
}
