using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.ChecklistItems.CreateBatch;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<IReadOnlyCollection<Create.Response>>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("checklist-items/batch");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToTodoCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            parentIssueNotFoundError =>
                Problem
                    .Failure("parentIssueId", "Parent issue not found", ErrorCodes.NotFound)
                    .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            checklistItems => TypedResults.Ok(checklistItems.ToResponse())
        );
    }
}
