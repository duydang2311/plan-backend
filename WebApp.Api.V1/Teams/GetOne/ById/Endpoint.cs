using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;
using WebApp.Features.Teams.GetOne;

namespace WebApp.Api.V1.Teams.GetOne.ById;

using Result = Results<ForbidHttpResult, NotFound, Ok<ResponseDto>>;

public sealed class Endpoint(IEnforcer enforcer) : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("teams/{Id}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (
            !await enforcer
                .EnforceAsync(req.UserId.ToString(), string.Empty, req.Id.ToString(), Permit.Read)
                .ConfigureAwait(false)
        )
        {
            return TypedResults.Forbid();
        }

        var oneOf = await new GetTeam { TeamId = req.Id, Select = req.Select, }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(_ => TypedResults.NotFound(), team => TypedResults.Ok(ResponseDto.From(team)));
    }
}
