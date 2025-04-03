using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ChatMessages.GetOne;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var chatMessage = await db
            .ChatMessages.Where(a => a.Id == context.Request.ChatMessageId)
            .Select(a => new { a.ChatId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var canRead =
            chatMessage is not null
            && await db
                .ChatMembers.AnyAsync(
                    a => a.ChatId == chatMessage.ChatId && a.MemberId == context.Request.RequestingUserId,
                    ct
                )
                .ConfigureAwait(false);
        if (!canRead)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
