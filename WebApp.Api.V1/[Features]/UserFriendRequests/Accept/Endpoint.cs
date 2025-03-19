using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.UserFriendRequests.Accept;

using Results = Results<NotFound, Conflict, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("user-friend-requests/accept");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => TypedResults.NotFound(),
            conflictError => TypedResults.Conflict(),
            success => TypedResults.NoContent()
        );
    }
}
