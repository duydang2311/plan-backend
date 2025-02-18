using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Domain.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueAudits.PatchComment;

public sealed class Authorize : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        return await dbContext
            .IssueAudits.AnyAsync(a => a.UserId == request.UserId && a.Action == IssueAuditAction.Comment, ct)
            .ConfigureAwait(false);
    }
}
