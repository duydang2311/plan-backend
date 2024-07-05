using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Workspaces.HasWorkspace;
using WebApp.SharedKernel.Models;

namespace WebApp.Api.V1.Workspaces.HasWorkspace;

using Results = Results<NotFound, Ok>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Head("workspaces");
        Verbs(Http.HEAD);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var has = await new HasWorkspaceCommand(
            new UserId(req.Sub),
            req.Id is not null ? new WorkspaceId(req.Id.Value) : null,
            req.Path
        )
            .ExecuteAsync(ct)
            .ConfigureAwait(false);
        return has ? TypedResults.Ok() : TypedResults.NotFound();
    }
}
