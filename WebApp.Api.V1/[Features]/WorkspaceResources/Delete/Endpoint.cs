using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.WorkspaceResources.Delete;

using Results = Results<ForbidHttpResult, InternalServerError<ProblemDetails>, NotFound, NoContent>;

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
        return oneOf.Match<Results>(
            serverError => serverError.ToProblemDetails(),
            notFoundError => TypedResults.NotFound(),
            success => TypedResults.NoContent()
        );
    }
}
