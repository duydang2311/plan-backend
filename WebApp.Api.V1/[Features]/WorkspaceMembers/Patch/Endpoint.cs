using FastEndpoints;
using JasperFx.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.WorkspaceMembers.Patch;

using Results = Results<ForbidHttpResult, ProblemDetails, NotFound<ProblemDetails>, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Patch("workspace-members/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return result.Match<Results>(
            notFound =>
                TypedResults.NotFound(
                    Problem
                        .Detail("Workspace member not found")
                        .StatusCode(StatusCodes.Status404NotFound)
                        .ToProblemDetails()
                ),
            forbiddenError => TypedResults.Forbid(),
            invalidPatch => Problem.Failure("patch", "Invalid patch", ErrorCodes.InvalidValue).ToProblemDetails(),
            success => TypedResults.NoContent()
        );
    }
}
