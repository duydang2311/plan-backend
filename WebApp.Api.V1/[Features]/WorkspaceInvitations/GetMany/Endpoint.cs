using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.WorkspaceInvitations.GetMany;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Get("workspace-invitations");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var list = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(list.ToResponse());
    }
}
