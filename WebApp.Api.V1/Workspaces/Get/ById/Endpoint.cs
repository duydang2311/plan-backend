using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Features.Workspaces.Get;

namespace WebApp.Api.V1.Workspaces.Get.ById;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(IEnforcer enforcer) : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (
            !await enforcer
                .EnforceAsync(
                    req.UserId.ToString(),
                    req.WorkspaceId.ToString(),
                    req.WorkspaceId.ToString(),
                    Permit.Read
                )
                .ConfigureAwait(false)
        )
        {
            return TypedResults.Forbid();
        }

        var oneOf = await new GetWorkspace { WorkspaceId = new WorkspaceId(req.WorkspaceId), Select = req.Select, }
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
