using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.TeamMembers.GetMany;

using Result = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Result>
{
    public override void Configure()
    {
        Get("teams/{TeamId}/members");
        Version(1);
    }

    public override async Task<Result> ExecuteAsync(Request req, CancellationToken ct)
    {
        var paginatedList = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok(paginatedList.ToResponse());
    }
}
