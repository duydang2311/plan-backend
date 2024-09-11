using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Projects.Create;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        var db = context.HttpContext.Resolve<AppDbContext>();
        var canCreate = await db
            .Users.AnyAsync(
                a =>
                    a.Id == context.Request.UserId
                    && a.Roles.Any(a =>
                        ((UserWorkspaceRole)a).WorkspaceId == context.Request.WorkspaceId
                        && a.Role.Permissions.Any(a => a.Permission.Equals(Permit.CreateProject))
                    ),
                ct
            )
            .ConfigureAwait(false);
        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
