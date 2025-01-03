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
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var workspaceId = await dbContext
            .Workspaces.Where(x => x.Path == req.Path)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        var oneOf = await new GetWorkspace { WorkspaceId = workspaceId, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(
            (_) => TypedResults.NotFound(),
            workspace => TypedResults.Ok(workspace.ToResponse())
        );
    }
}
