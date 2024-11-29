using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Projects.GetMany;

using Results = Results<ForbidHttpResult, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("projects");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(list => TypedResults.Ok(list.ToResponse()));
    }
}
