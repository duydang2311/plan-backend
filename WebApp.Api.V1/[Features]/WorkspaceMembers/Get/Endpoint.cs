using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.WorkspaceMembers.Get;

using Result = Results<ForbidHttpResult, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}/members");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Result>(workspace => TypedResults.Ok(workspace.ToResponse()));
    }
}
