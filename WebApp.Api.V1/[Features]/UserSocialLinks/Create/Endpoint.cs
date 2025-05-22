using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Helpers;

namespace WebApp.Api.V1.UserSocialLinks.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("user-social-links");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError =>
                Problem.Detail("User not found").ToProblemDetails(statusCode: StatusCodes.Status404NotFound),
            userSocialLink => TypedResults.Ok(userSocialLink.ToResponse())
        );
    }
}
