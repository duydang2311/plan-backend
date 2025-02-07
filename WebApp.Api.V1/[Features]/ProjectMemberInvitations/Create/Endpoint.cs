using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Models;

namespace WebApp.Api.V1.ProjectMemberInvitations.Create;

using Results = Results<
    ForbidHttpResult,
    UnprocessableEntity<ProblemDetails>,
    Conflict<ProblemDetails>,
    Conflict,
    NoContent
>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("project-member-invitations");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToUnprocessableEntity(),
            alreadyMemberError =>
                Problem.Failure("root", "User is already a member", "member_already").ToProblemDetails().ToConflict(),
            conflictError =>
                Problem
                    .Failure("root", "Invitation is already created", "invitation_conflict")
                    .ToProblemDetails()
                    .ToConflict(),
            success => TypedResults.NoContent()
        );
    }
}
