using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.UserProfiles.ImageUploaded;

public sealed class Endpoint : Endpoint<Request, Ok>
{
    public override void Configure()
    {
        Post("users/{UserId}/profile/image-uploaded");
        AllowAnonymous();
    }

    public override async Task<Ok> ExecuteAsync(Request req, CancellationToken ct)
    {
        await req.ToJob().QueueJobAsync(ct: CancellationToken.None).ConfigureAwait(false);
        return TypedResults.Ok();
    }
}
