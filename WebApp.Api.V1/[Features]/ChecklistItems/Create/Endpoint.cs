using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Constants;
using WebApp.Domain.Constants;

namespace WebApp.Api.V1.ChecklistItems.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("checklist-items");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        switch (req.Kind)
        {
            case ChecklistItemKind.Todo:
            {
                var oneOf = await req.ToTodoCommand().ExecuteAsync(ct).ConfigureAwait(false);
                return oneOf.Match<Results>(
                    parentIssueNotFoundError =>
                        Problem
                            .Failure("parentIssueId", "Parent issue not found", ErrorCodes.NotFound)
                            .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
                    checklistItem => TypedResults.Ok(checklistItem.ToResponse())
                );
            }
            case ChecklistItemKind.SubIssue:
            {
                var oneOf = await req.ToSubIssueCommand().ExecuteAsync(ct).ConfigureAwait(false);
                return oneOf.Match<Results>(
                    parentIssueNotFoundError =>
                        Problem
                            .Failure("parentIssueId", "Parent issue not found", ErrorCodes.NotFound)
                            .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
                    subIssueNotFoundError =>
                        Problem
                            .Failure("subIssueId", "Sub issue not found", ErrorCodes.NotFound)
                            .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
                    duplicatedError =>
                        Problem
                            .Multiple(2)
                            .Failure(
                                "parentIssueId",
                                "'parentIssueId' and 'subIssueId' must be unique",
                                ErrorCodes.Duplicated
                            )
                            .Failure(
                                "subIssueId",
                                "'parentIssueId' and 'subIssueId' must be unique",
                                ErrorCodes.Duplicated
                            )
                            .ToProblemDetails(statusCode: StatusCodes.Status409Conflict),
                    checklistItem => TypedResults.Ok(checklistItem.ToResponse())
                );
            }
            default:
                throw new InvalidOperationException();
        }
    }
}
