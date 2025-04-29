using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.ResourceFiles.CreateBatch;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("resource-files/batch");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            resourceNotFoundError =>
                Problem.Failure("resourceId", "Resource not found", ErrorCodes.NotFound).ToProblemDetails(),
            ids => TypedResults.Ok(ids.ToResponse())
        );
    }
}
