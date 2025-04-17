using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceInvitations.Accept;

public sealed record AcceptWorkspaceInvitationHandler(AppDbContext db, ILogger<AcceptWorkspaceInvitationHandler> logger)
    : ICommandHandler<AcceptWorkspaceInvitation, OneOf<NotFoundError, ConflictError, Success>>
{
    public async Task<OneOf<NotFoundError, ConflictError, Success>> ExecuteAsync(
        AcceptWorkspaceInvitation command,
        CancellationToken ct
    )
    {
        var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);

        var invitation = await db
            .WorkspaceInvitations.Where(a => a.Id == command.Id)
            .Select(a => new { a.UserId, a.WorkspaceId })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (invitation is null)
        {
            return new NotFoundError();
        }

        var count = await db
            .WorkspaceInvitations.Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }

        var workspaceMember = new WorkspaceMember { UserId = invitation.UserId, WorkspaceId = invitation.WorkspaceId };
        db.Add(workspaceMember);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            await transaction.CommitAsync(ct).ConfigureAwait(false);
        }
        catch (UniqueConstraintException)
        {
            return new ConflictError();
        }

        return new Success();
    }
}
