using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Models;

namespace WebApp.Api.V1.WorkspaceInvitations.Create;

using Results = Results<ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspace/{WorkspaceId}/invitations");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(failures => failures.ToProblemDetails(), success => TypedResults.NoContent());
    }
}
