using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.Delete;

public sealed record DeleteChecklistItemHandler(AppDbContext db)
    : ICommandHandler<DeleteChecklistItem, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteChecklistItem command, CancellationToken ct)
    {
        var count = await db.ChecklistItems.Where(x => x.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
