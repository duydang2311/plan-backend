using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Models;

namespace WebApp.Api.V1.Projects.GetOne.ById;

using Results = Results<ForbidHttpResult, ProblemDetails, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("projects/{ProjectId}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            none => TypedResults.NotFound(),
            project => TypedResults.Ok(project.ToResponse())
        );
    }
}
