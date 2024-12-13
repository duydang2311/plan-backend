using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceInvitations.Create;

public sealed class CreateWorkspaceInvitationHandler(AppDbContext db)
    : ICommandHandler<CreateWorkspaceInvitation, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(
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
                    _ => null
                }
            );
        }
        catch (UniqueConstraintException e)
        {
            return e.ToValidationFailures(propertyName =>
                propertyName switch
                {
                    nameof(WorkspaceInvitation.UserId) => ("userId", "User is already invited"),
                    _ => null
                }
            );
        }

        return new Success();
    }
}
