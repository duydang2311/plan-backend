using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Issues.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("issues");
        Verbs(Http.POST);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            issue => TypedResults.Ok(issue.ToResponse())
        );
    }
}
