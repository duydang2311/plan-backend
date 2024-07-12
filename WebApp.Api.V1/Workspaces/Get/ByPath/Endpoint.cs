using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Features.Workspaces.Get;
using WebApp.SharedKernel.Constants;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Api.V1.Workspaces.Get.ByPath;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(IEnforcer enforcer, AppDbContext dbContext) : Endpoint<Request, Result>
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
        if (
            !await enforcer
                .EnforceAsync(req.UserId.ToString(), workspaceId.ToString(), workspaceId.ToString(), Permit.Read)
                .ConfigureAwait(false)
        )
        {
            return TypedResults.Forbid();
        }

        var oneOf = await new GetWorkspace { WorkspaceId = workspaceId, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(
            (_) => TypedResults.NotFound(),
            workspace =>
                TypedResults.Ok(
                    new Response
                    {
                        CreatedTime = workspace.CreatedTime,
                        UpdatedTime = workspace.UpdatedTime,
                        Id = workspace.Id.Value,
                        Name = workspace.Name,
                        Path = workspace.Path,
                    }
                )
        );
    }
}
