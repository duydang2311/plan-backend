using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Api.V1.WorkspaceResources.Delete;

using Results = Results<ForbidHttpResult, NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Delete("workspace-resources/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request request, CancellationToken ct)
    {
        var oneOf = await request.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(notFoundError => TypedResults.NotFound(), success => TypedResults.NoContent());
    }
}
