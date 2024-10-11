using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceStatuses.Delete;

public sealed class DeleteWorkspaceStatusHandler(AppDbContext db)
    : ICommandHandler<DeleteWorkspaceStatus, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteWorkspaceStatus command, CancellationToken ct)
    {
        var count = await db
            .WorkspaceStatuses.Where(a => a.Id == command.StatusId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
