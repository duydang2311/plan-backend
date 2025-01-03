using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Teams.GetOne;

namespace WebApp.Api.V1.Teams.GetOne.ById;

using Result = Results<ForbidHttpResult, NotFound, Ok<ResponseDto>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("teams/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await new GetTeam { TeamId = req.Id, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(_ => TypedResults.NotFound(), team => TypedResults.Ok(ResponseDto.From(team)));
    }
}
