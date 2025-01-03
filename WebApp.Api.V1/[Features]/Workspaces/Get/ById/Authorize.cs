using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Workspaces.Get.ById;

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
            .WorkspaceMembers.AnyAsync(a => a.UserId == request.UserId && a.WorkspaceId == request.WorkspaceId, ct)
            .ConfigureAwait(false);
    }
}
