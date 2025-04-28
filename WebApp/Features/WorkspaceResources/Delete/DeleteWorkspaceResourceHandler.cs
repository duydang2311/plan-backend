using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.Delete;

public sealed record DeleteWorkspaceResourceHandler(AppDbContext db)
    : ICommandHandler<DeleteWorkspaceResource, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteWorkspaceResource command, CancellationToken ct)
    {
        var count = await db
            .WorkspaceResources.Where(a => a.ResourceId == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
