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

    public override async Task<Results> ExecuteAsync(Request request, CancellationToken ct)
    {
        var result = await new AuthenticateCommand(request.Email!, request.Password!).ExecuteAsync(ct).ConfigureAwait(false);
        return result.Match<Results>(
            (a) => a,
            (a) => TypedResults.Ok(new Response(
                a.AccessToken,
                a.RefreshToken,
                a.AccessTokenMaxAge,
                a.RefreshTokenMaxAge)));
    }
}
