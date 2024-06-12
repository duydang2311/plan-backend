using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Users.CreateUser;

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
        return result.Match<Results>((errors) => errors.ToProblemDetails(400), (_) => TypedResults.NoContent());
    }
}
