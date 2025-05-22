using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.UserSocialLinks.Delete;

public sealed class Authorize : IPreProcessor<Request>
{
    public async Task PreProcessAsync(IPreProcessorContext<Request> context, CancellationToken ct)
    {
        if (context.Request is null || context.HasValidationFailures)
        {
            return;
        }

        var db = context.HttpContext.Resolve<AppDbContext>();
        var userSocialLink = await db
            .UserSocialLinks.Where(a => a.Id == context.Request.Id)
            .Select(a => new { a.UserId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (userSocialLink is null || userSocialLink.UserId != context.Request.RequestingUserId)
        {
            await context.HttpContext.Response.SendForbiddenAsync(ct).ConfigureAwait(false);
        }
    }
}
