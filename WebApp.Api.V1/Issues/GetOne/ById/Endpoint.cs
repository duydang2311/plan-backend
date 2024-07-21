using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetOne.ById;

using Results = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(AppDbContext dbContext, IEnforcer enforcer) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("issues/{IssueId}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var teamId = await dbContext.Issues
            .Where(x => x.Id == req.IssueId)
            .Select(x => x.TeamId)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (teamId == TeamId.Empty || !await enforcer.EnforceAsync(req.UserId.ToString(), teamId.ToString(), req.IssueId.ToString(), Permit.Read))
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
