using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Common.Models;

namespace WebApp.Api.V1.TeamInvitations.Accept;

using Results = Results<ForbidHttpResult, ProblemDetails, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Patch("team-invitations/{TeamInvitationId}/accept");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(failures => failures.ToProblemDetails(), _ => TypedResults.NoContent());
    }
}
