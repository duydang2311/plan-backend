using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Models;

namespace WebApp.Api.V1.Tokens.Authenticate.WithGoogle;

using Results = Results<ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("tokens/authenticate/google");
        Version(1);
        AllowAnonymous();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return result.Match<Results>(
            failures => failures.ToProblemDetails(),
            result => TypedResults.Ok(result.ToResponse())
        );
    }
}
