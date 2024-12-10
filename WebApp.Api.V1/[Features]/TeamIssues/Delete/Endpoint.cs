using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.TeamIssues.Delete;

using Results = Results<NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Delete("team-issues/{TeamId}/{IssueId}");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(notFoundError => TypedResults.NotFound(), success => TypedResults.NoContent());
    }
}
