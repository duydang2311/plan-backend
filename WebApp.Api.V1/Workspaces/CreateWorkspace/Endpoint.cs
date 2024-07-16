using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Features.Workspaces.CreateWorkspace;

namespace WebApp.Api.V1.Workspaces.CreateWorkspace;

using Results = Results<ProblemDetails, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspaces");
        Verbs(Http.POST);
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await new CreateWorkspaceCommand(req.UserId, req.Name!, req.Path!)
            .ExecuteAsync(ct)
            .ConfigureAwait(false);
        return oneOf.Match<Results>(
            (x) => x.ToProblemDetails(statusCode: 400),
            (x) => TypedResults.Ok(new Response(x.Id.Value, x.Path))
        );
    }
}
