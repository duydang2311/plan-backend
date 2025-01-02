using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Features.Workspaces.Get;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Workspaces.Get.ByPath;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(AppDbContext dbContext) : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("workspaces/path/{Path}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var workspaceId = await dbContext
            .Workspaces.Where(x => x.Path == req.Path)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        // TODO: Authorize
        // if (
        //     !await enforcer
        //         .EnforceAsync(req.UserId.ToString(), workspaceId.ToString(), workspaceId.ToString(), Permit.Read)
        //         .ConfigureAwait(false)
        // )
        // {
        //     return TypedResults.Forbid();
        // }

        var oneOf = await new GetWorkspace { WorkspaceId = workspaceId, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(
            (_) => TypedResults.NotFound(),
            workspace => TypedResults.Ok(workspace.ToResponse())
        );
    }
}
