using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetOne;

using Results = Results<ForbidHttpResult, NotFound, Ok<Response>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("project-member-invitations/{ProjectMemberInvitationId}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => TypedResults.NotFound(),
            projectMemberInvitation => TypedResults.Ok(projectMemberInvitation.ToResponse())
        );
    }
}
