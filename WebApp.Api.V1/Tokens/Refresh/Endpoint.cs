using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Domain.Entities;
using WebApp.Features.Tokens.Refresh;

namespace WebApp.Api.V1.Tokens.Refresh;

using Results = Results<ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        AllowAnonymous();
        Verbs(Http.POST);
        Version(1);
        Post("tokens/refresh");
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await new RefreshCommand(new RefreshToken(req.RefreshToken!.Value))
            .ExecuteAsync(ct)
            .ConfigureAwait(false);
        return result.Match<Results>(
            (errors) => errors.ToProblemDetails(400),
            (result) => TypedResults.Ok(new Response(result.AccessToken, result.AccessTokenMaxAge))
        );
    }
}
