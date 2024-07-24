using Casbin;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueComments.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, NoContent>;

public sealed class Endpoint(AppDbContext dbContext, IEnforcer enforcer) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("issues/{IssueId}/comments");
        Verbs(Http.POST);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var teamId = await dbContext.Issues
            .Where(x => x.Id == req.IssueId)
            .Select(x => x.TeamId)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (
            teamId == TeamId.Empty || !await enforcer.EnforceAsync(
                req.AuthorId.ToString(),
                teamId.ToString(),
                req.IssueId.ToString(),
                Permit.CommentIssue
            )
        )
        {
            return TypedResults.Forbid();
        }

        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            failures => failures.ToProblemDetails(),
            _ => TypedResults.NoContent()
        );
    }
}
