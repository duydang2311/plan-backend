using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Models;

namespace WebApp.Api.V1.Projects.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Conflict, InternalServerError<ProblemDetails>, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("projects");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            conflict => TypedResults.Conflict(),
            failures => failures.ToProblemDetails(),
            serverError => serverError.ToProblemDetails(),
            project => TypedResults.Ok(project.ToResponse())
        );
    }
}
