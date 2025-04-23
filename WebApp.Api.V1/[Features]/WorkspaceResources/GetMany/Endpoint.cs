using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Api.V1.WorkspaceResources.GetMany;

using Results = Results<ForbidHttpResult, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("workspaces/{WorkspaceId}/resources");
        Version(1);
    }

    public override async Task<Results> ExecuteAsync(Request request, CancellationToken ct)
    {
        var list = await request.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return TypedResults.Ok(list.ToResponse());
    }
}
