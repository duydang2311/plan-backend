using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Models;

namespace WebApp.Api.V1.UserFriends.Create;

using Results = Results<ProblemDetails, Conflict, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("user-friends");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            validationFailures => validationFailures.ToProblemDetails(),
            duplicatedError => TypedResults.Conflict(),
            success => TypedResults.NoContent()
        );
    }
}
