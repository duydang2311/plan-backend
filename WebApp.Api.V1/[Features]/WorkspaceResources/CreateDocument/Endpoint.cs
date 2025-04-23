using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.WorkspaceResources.CreateDocument;

using Results = Results<ForbidHttpResult, ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspace-resources/documents");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return oneOf.Match<Results>(
            workspaceNotFoundError =>
                Problem.Failure("workspaceId", "Workspace not found", ErrorCodes.NotFound).ToProblemDetails(),
            userNotFoundError =>
                Problem.Failure("creator", "Creator not found", ErrorCodes.NotFound).ToProblemDetails(),
            invalidResourceTypeError =>
                Problem.Failure("type", "Invalid resource type", ErrorCodes.InvalidValue).ToProblemDetails(),
            success => TypedResults.NoContent()
        );
    }
}
