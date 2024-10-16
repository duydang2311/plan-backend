using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.WorkspaceStatuses.Patch;

using Results = Results<ForbidHttpResult, NotFound, ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Patch("workspace-statuses/{StatusId}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            notFoundError => TypedResults.NotFound(),
            success => TypedResults.NoContent()
        );
    }
}
