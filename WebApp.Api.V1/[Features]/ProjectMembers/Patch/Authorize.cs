using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMembers.Patch;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var projectMember = await db
            .ProjectMembers.Where(a => a.Id == context.Request.ProjectMemberId)
            .Select(a => new { a.ProjectId, RoleId = a.Role.Id })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (projectMember is null)
        {
            await context.HttpContext.Response.SendNotFoundAsync(ct).ConfigureAwait(false);
            return;
        }

        var canPatch = await db
            .ProjectMembers.AnyAsync(
                a =>
                    a.UserId == context.Request.RequestingUserId
                    && a.ProjectId == projectMember.ProjectId
                    && a.Role.Permissions.Any(b => b.Permission.Equals(Permit.UpdateProjectMember))
                    && a.RoleId > projectMember.RoleId,
                ct
            )
            .ConfigureAwait(false);
        if (!canPatch)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
