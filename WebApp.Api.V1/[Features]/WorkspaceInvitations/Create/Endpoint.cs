using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Models;

namespace WebApp.Api.V1.WorkspaceInvitations.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, InternalServerError<ProblemDetails>, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspaces/{WorkspaceId}/invitations");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            serverError => serverError.ToProblemDetails(),
            success => TypedResults.NoContent()
        );
    }
}
