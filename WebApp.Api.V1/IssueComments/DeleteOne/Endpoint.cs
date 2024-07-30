using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.IssueComments.DeleteOne;

using Results = Results<ForbidHttpResult, NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Delete("issue-comments/{IssueCommentId}");
        Verbs(Http.DELETE);
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results<ForbidHttpResult, NotFound, NoContent>> ExecuteAsync(
        Request req,
        CancellationToken ct
    )
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(failures => TypedResults.NotFound(), _ => TypedResults.NoContent());
    }
}
