using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.ProjectMembers.GetMetadata;

using Results = Results<ForbidHttpResult, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("projects/{ProjectId}/members/metadata");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(result.ToResponse());
    }
}
