using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Tokens.Authenticate;

namespace WebApp.Api.V1.Tokens.Authenticate;

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
        var result = await new AuthenticateCommand(req.Email!, req.Password!).ExecuteAsync(ct).ConfigureAwait(false);
        return result.Match<Results>(
            (errors) => errors.ToProblemDetails(400),
            (result) =>
                TypedResults.Ok(
                    new Response(
                        result.AccessToken,
                        result.RefreshToken.Value,
                        result.AccessTokenMaxAge,
                        result.RefreshTokenMaxAge
                    )
                )
        );
    }
}
