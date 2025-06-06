using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.Milestones.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("milestones");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            projectNotFoundError =>
                Problem
                    .Failure("projectId", "Project not found", ErrorCodes.NotFound)
                    .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            milestoneStatusNotFoundError =>
                Problem
                    .Failure("statusId", "Status not found", ErrorCodes.NotFound)
                    .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            milestone => TypedResults.Ok(milestone.ToResponse())
        );
    }
}
