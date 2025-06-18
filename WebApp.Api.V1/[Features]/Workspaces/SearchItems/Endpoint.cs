using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Workspaces.SearchItems;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}/search");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok(result.ToResponse());
    }
}
