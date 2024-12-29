using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Workspaces.GetMany;

public sealed class Endpoint : Endpoint<Request, Results<ForbidHttpResult, Ok<Response>>>
{
    public override void Configure()
    {
        Get("workspaces");
        PreProcessor<Authorize>();
        Version(1);
    }

    public override async Task<Results<ForbidHttpResult, Ok<Response>>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var list = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(list.ToResponse());
    }
}
