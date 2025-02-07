using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Features.Teams.Create;

namespace WebApp.Api.V1.Teams.Create;

using Result = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Post("teams");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await new CreateTeam
        {
            UserId = req.UserId,
            WorkspaceId = req.WorkspaceId!.Value,
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
