using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.UserFriends.Search;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Get("user-friends/search");
        Version(1);
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok(result.ToResponse());
    }
}
