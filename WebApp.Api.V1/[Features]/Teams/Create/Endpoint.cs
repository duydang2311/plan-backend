using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Constants;
using WebApp.Common.Models;
using WebApp.Features.Teams.Create;

namespace WebApp.Api.V1.Teams.Create;

using Result = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint(IEnforcer enforcer) : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Post("teams");
        Verbs(Http.POST);
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        if (
            !await enforcer
                .EnforceAsync(
                    req.UserId.ToString(),
                    req.WorkspaceId!.Value.ToString(),
                    req.WorkspaceId.Value.ToString(),
                    Permit.WriteTeam
                )
                .ConfigureAwait(false)
        )
        {
            return TypedResults.Forbid();
        }

        var oneOf = await new CreateTeam
        {
            UserId = req.UserId,
            WorkspaceId = req.WorkspaceId.Value,
            Name = req.QualifiedName!,
            Identifier = req.QualifiedIdentifier!,
        }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(
            failures => failures.ToProblemDetails(),
            team => TypedResults.Ok(new Response { TeamId = team.Id.Value, Identifier = team.Identifier })
        );
    }
}
