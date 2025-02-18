using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.IssueAudits.PatchComment;

using Results = Results<ForbidHttpResult, NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Patch("issue-audits/comments/{Id}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(notFoundError => TypedResults.NotFound(), success => TypedResults.NoContent());
    }
}
