using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.Create;

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
            .ProjectMembers.AnyAsync(
                a =>
                    a.UserId == request.RequestingUserId
                    && a.ProjectId == request.ProjectId
                    && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.CreateProjectMemberInvitation)),
                ct
            )
            .ConfigureAwait(false);
    }
}
