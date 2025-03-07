using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.ProjectMemberInvitations.Accept;

using Results = Results<ForbidHttpResult, NotFound, NoContent>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("project-member-invitations/{ProjectMemberInvitationId}/accept");
        Version(1);
        PreProcessor<Authorize>();
        Description(a => a.Accepts<Request>());
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(notFound => TypedResults.NotFound(), success => TypedResults.NoContent());
    }
}
