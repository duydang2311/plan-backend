using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Api.V1.Workspaces.GetAnalytics;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}/analytics");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request request, CancellationToken ct)
    {
        var result = await request.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(result.ToResponse());
    }
}
