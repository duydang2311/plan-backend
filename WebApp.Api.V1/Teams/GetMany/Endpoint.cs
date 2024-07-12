using FastEndpoints;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Teams.GetMany;

namespace WebApp.Api.V1.Teams.GetMany;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("teams");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var paginatedList = await new GetTeams
        {
            UserId = req.UserId,
            WorkspaceId = req.WorkspaceId,
            Select = req.Select,
        }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return TypedResults.Ok(
            new Response { TotalCount = paginatedList.TotalCount, Items = [.. paginatedList.Items.Select(Item.From)] }
        );
    }
}
