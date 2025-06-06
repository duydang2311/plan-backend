using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Models;

namespace WebApp.Api.V1.Users.GetOne.ByProfileName;

using Results = Results<ForbidHttpResult, ProblemDetails, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("users/profile-name/{ProfileName}");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            notFoundError => TypedResults.NotFound(),
            user => TypedResults.Ok(user.ToResponse())
        );
    }
}
