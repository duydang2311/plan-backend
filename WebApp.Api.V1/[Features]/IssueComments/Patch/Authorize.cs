using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.IssueComments.Patch;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null)
        {
            return;
        }

        var dbContext = context.HttpContext.Resolve<AppDbContext>();
        var isAuthor = await dbContext
            .IssueComments.AnyAsync(x => x.AuthorId == context.Request.UserId, ct)
            .ConfigureAwait(false);
        if (!isAuthor)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
