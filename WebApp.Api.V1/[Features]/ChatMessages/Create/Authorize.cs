using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ChatMessages.Create;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var canCreate = await db
            .ChatMembers.AnyAsync(
                a => a.ChatId == context.Request.ChatId && a.MemberId == context.Request.RequestingUserId,
                ct
            )
            .ConfigureAwait(false);
        if (!canCreate)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
