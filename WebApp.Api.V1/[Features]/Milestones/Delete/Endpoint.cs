using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.Milestones.Delete;

using Results = Results<ForbidHttpResult, ProblemDetails, NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Delete("milestones/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return oneOf.Match<Results>(
            notFoundError =>
                Problem.Detail("Milestone not found").ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            success => TypedResults.NoContent()
        );
    }
}
