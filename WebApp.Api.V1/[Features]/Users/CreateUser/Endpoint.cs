using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OneOf;
using WebApp.Features.Users.CreateUser;
using WebApp.Features.Users.SendVerificationMail;

namespace WebApp.Api.V1.Users.CreateUser;

using Results = Results<ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        AllowAnonymous();
        Verbs(Http.POST);
        Version(1);
        Post("users");
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var result = await new CreateUserCommand(req.Email!, req.Password!).ExecuteAsync(ct).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(req.VerificationUrl) && result.TryGetT1(out var createUserResult))
        {
            await new SendUserVerificationMail(
                createUserResult.User.Email,
                req.VerificationUrl,
                createUserResult.UserVerificationToken.Token
            )
                .QueueJobAsync(ct: ct)
                .ConfigureAwait(false);
        }
        return result.Match<Results>((errors) => errors.ToProblemDetails(400), (_) => TypedResults.NoContent());
    }
}
