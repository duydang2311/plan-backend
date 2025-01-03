using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Features.Teams.GetOne;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Teams.GetOne.ByIdentifier;

using Result = Results<ForbidHttpResult, NotFound, Ok<ResponseDto>>;

public sealed class Endpoint(AppDbContext dbContext) : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}/teams/identifier/{Identifier}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var teamId = await dbContext
            .Teams.Where(x => x.WorkspaceId == req.WorkspaceId && x.Identifier.Equals(req.Identifier))
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (teamId == TeamId.Empty)
        {
            return TypedResults.NotFound();
        }

        var oneOf = await new GetTeam
        {
            WorkspaceId = req.WorkspaceId,
            Identifier = req.Identifier,
            Select = req.Select,
        }
            .ExecuteAsync(ct)
            .ConfigureAwait(false);

        return oneOf.Match<Result>(_ => TypedResults.NotFound(), team => TypedResults.Ok(ResponseDto.From(team)));
    }
}
