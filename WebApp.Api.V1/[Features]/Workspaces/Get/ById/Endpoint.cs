using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Workspaces.Get;

namespace WebApp.Api.V1.Workspaces.Get.ById;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await new GetWorkspace { WorkspaceId = req.WorkspaceId, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(
            (_) => TypedResults.NotFound(),
            workspace => TypedResults.Ok(workspace.ToResponse())
        );
    }
}
