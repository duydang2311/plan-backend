using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.WorkspaceResources.CreateUploadUrl;

using Results = Results<ForbidHttpResult, ProblemDetails, InternalServerError<ProblemDetails>, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspace-resources/upload-url");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => Problem.Failure("workspaceId", "Workspace not found").ToProblemDetails(),
            serverError => serverError.ToProblemDetails(),
            result => TypedResults.Ok(result.ToResponse())
        );
    }
}
