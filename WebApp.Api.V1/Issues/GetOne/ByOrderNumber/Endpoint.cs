using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Issues.GetOne.ByOrderNumber;

using Results = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint(AppDbContext dbContext, IEnforcer enforcer) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("issues/teams/{TeamId}/order-numbers/{OrderNumber}");
        Verbs(Http.GET);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var issueId = await dbContext.Issues
            .Where(x => x.TeamId == req.TeamId && x.OrderNumber == req.OrderNumber)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (issueId == IssueId.Empty)
        {
            return TypedResults.NotFound();
        }

        if (!await enforcer.EnforceAsync(req.UserId.ToString(), string.Empty, issueId.ToString(), Permit.Read))
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
