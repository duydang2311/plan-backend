using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;
using Wolverine.EntityFrameworkCore;

namespace WebApp.Features.WorkspaceInvitations.Create;

public sealed class CreateWorkspaceInvitationHandler(
    AppDbContext db,
    ILogger<CreateWorkspaceInvitationHandler> logger,
    IDbContextOutbox outbox
) : ICommandHandler<CreateWorkspaceInvitation, OneOf<ValidationFailures, ServerError, Success>>
{
    public async Task<OneOf<ValidationFailures, ServerError, Success>> ExecuteAsync(
        CreateWorkspaceInvitation command,
        CancellationToken ct
    )
    {
        if (
            await db
                .WorkspaceMembers.AnyAsync(a => a.WorkspaceId == command.WorkspaceId && a.UserId == command.UserId, ct)
                .ConfigureAwait(false)
        )
        {
            return ValidationFailures.Single("userId", "User is already a member", "member_already");
        }

        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
        var invitation = new WorkspaceInvitation { WorkspaceId = command.WorkspaceId, UserId = command.UserId };
        db.Add(invitation);

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(propertyName =>
                propertyName switch
                {
                    nameof(WorkspaceInvitation.WorkspaceId) => ("workspaceId", "Workspace does not exist"),
                    nameof(WorkspaceInvitation.UserId) => ("userId", "User does not exist"),
                    _ => null,
                }
            );
        }
        catch (UniqueConstraintException e)
        {
            return e.ToValidationFailures(propertyName =>
                propertyName switch
                {
                    nameof(WorkspaceInvitation.UserId) => ("userId", "User is already invited"),
                    _ => null,
                }
            );
        }

        try
        {
            outbox.Enroll(db);
            await outbox
                .PublishAsync(
                    new WorkspaceMemberInvited { WorkspaceInvitationId = invitation.Id, MemberId = command.UserId }
                )
                .ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish WorkspaceInvitationCreated event");
            return Errors.Outbox();
        }

        return new Success();
    }
}
