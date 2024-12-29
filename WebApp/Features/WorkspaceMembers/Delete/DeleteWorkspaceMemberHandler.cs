using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.Delete;

public sealed class DeleteWorkspaceMemberHandler(AppDbContext db)
    : ICommandHandler<DeleteWorkspaceMember, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteWorkspaceMember command, CancellationToken ct)
    {
        var count = await db
            .WorkspaceMembers.Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
