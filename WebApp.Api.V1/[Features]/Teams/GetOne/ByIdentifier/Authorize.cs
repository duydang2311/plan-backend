using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Teams.GetOne.ByIdentifier;

public sealed class Authorize : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        return await db
                .WorkspaceMembers.AnyAsync(
                    a =>
                        a.WorkspaceId == request.WorkspaceId
                        && a.UserId == request.UserId
                        && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.ReadTeam)),
                    ct
                )
                .ConfigureAwait(false)
            || await db
                .TeamMembers.AnyAsync(
                    a => a.Team.WorkspaceId == request.WorkspaceId && a.Team.Identifier.Equals(request.Identifier),
                    ct
                )
                .ConfigureAwait(false);
    }
}
