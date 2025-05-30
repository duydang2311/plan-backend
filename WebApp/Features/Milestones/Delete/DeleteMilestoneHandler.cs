using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Milestones.Delete;

public sealed record DeleteMilestoneHandler(AppDbContext db)
    : ICommandHandler<DeleteMilestone, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteMilestone command, CancellationToken ct)
    {
        var count = await db.Milestones.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
