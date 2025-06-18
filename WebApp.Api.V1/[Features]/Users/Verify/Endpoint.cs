using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.Users.Verify;

using Results = Results<ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("users/verify/{Token}");
        Version(1);
        AllowAnonymous();
        Description(a => a.ClearDefaultAccepts().Accepts<Request>(isOptional: true, "*/*"));
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFound =>
                Problem
                    .Detail("Verification token not found.")
                    .ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            success => TypedResults.NoContent()
        );
    }
}
