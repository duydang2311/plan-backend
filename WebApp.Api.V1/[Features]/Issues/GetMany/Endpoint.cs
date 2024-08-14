using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.Issues.GetMany;

using Results = Results<ForbidHttpResult, Ok<Response>>;

public sealed class Endpoint(IEnforcer enforcer) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("issues");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (req.TeamId is null)
        {
            return TypedResults.Forbid();
        }

        if (
            req.TeamId is not null
            && !await enforcer
                .EnforceAsync(req.UserId.ToString(), string.Empty, req.TeamId.ToString(), Permit.Read)
                .ConfigureAwait(false)
        )
        {
            return TypedResults.Forbid();
        }

        var list = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(list.ToResponse());
    }
}
