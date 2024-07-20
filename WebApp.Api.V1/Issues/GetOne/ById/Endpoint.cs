using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;

namespace WebApp.Api.V1.Issues.GetOne.ById;

using Results = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(IEnforcer enforcer) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("issues/{IssueId}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (!await enforcer.EnforceAsync(req.UserId.ToString(), string.Empty, req.IssueId.ToString(), Permit.Read))
        {
            return TypedResults.Forbid();
        }

        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            (_) => TypedResults.NotFound(),
            (issue) => TypedResults.Ok(issue.ToResponse())
        );
    }
}
