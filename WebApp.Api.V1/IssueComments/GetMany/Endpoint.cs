using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Api.V1.IssueComments.GetMany;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Get("issues/{IssueId}/comments");
        Verbs(Http.GET);
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var list = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(list.ToResponse());
    }
}
