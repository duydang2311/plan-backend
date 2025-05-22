using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Users.SignProfileImageUpload;

public sealed class Endpoint : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Post("users/profiles/signed-upload");
        Version(1);
        Description(a => a.ClearDefaultAccepts().Accepts<Request>(isOptional: true, "*/*"));
    }

    public override async Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(result.ToResponse());
    }
}
