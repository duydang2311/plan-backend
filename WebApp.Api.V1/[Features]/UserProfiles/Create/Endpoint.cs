using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.UserProfiles.Create;

using Results = Results<ForbidHttpResult, NotFound, Conflict, ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("users/{UserId}/profile");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => TypedResults.NotFound(),
            duplicatedError => TypedResults.Conflict(),
            failures => failures.ToProblemDetails(),
            success => TypedResults.NoContent()
        );
    }
}
