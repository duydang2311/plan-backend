using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Tokens.Authenticate.WithCredentials;

using Results = Results<ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        AllowAnonymous();
        Verbs(Http.POST);
        Version(1);
        Post("tokens/authenticate");
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
