using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.WorkspaceInvitations.Accept;

using Results = Results<ForbidHttpResult, NotFound, Conflict<ProblemDetails>, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspace-invitations/{Id}/accept");
        Version(1);
        PreProcessor<Authorize>();
        Description(a => a.ClearDefaultAccepts().Accepts<Request>(isOptional: true, "*/*"));
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => TypedResults.NotFound(),
            conflictError =>
                TypedResults.Conflict(Problem.Failure("userId", "User is already a member").ToProblemDetails()),
            success => TypedResults.NoContent()
        );
    }
}
