using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.ChecklistItems.Patch;

using Results = Results<ForbidHttpResult, ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Patch("checklist-items/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return oneOf.Match<Results>(
            invalidPatchError => Problem.Failure("patch", "Invalid patch", ErrorCodes.InvalidValue).ToProblemDetails(),
            notFoundError =>
                Problem.Detail("Checklist item not found").ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            success => TypedResults.NoContent()
        );
    }
}
