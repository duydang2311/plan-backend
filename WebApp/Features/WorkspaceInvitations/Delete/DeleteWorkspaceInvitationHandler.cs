using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceInvitations.Delete;

public sealed class DeleteWorkspaceInvitationHandler(AppDbContext db)
    : ICommandHandler<DeleteWorkspaceInvitation, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(
        DeleteWorkspaceInvitation command,
        CancellationToken ct
    )
    {
        var count = await db
            .WorkspaceInvitations.Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
