using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.Projects.Create;

using Results = Results<ForbidHttpResult, ProblemDetails, Conflict, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("projects");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        return base.ExecuteAsync(req, ct);
    }
}
