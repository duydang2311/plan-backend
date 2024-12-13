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
            return ValidationFailures
                .Many(2)
                .Add("workspace_id", "User is already a member")
                .Add("user_id", "User is already a member");
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
                    nameof(WorkspaceInvitation.WorkspaceId) => ("workspace_id", "Workspace does not exist"),
                    nameof(WorkspaceInvitation.UserId) => ("user_id", "User does not exist"),
                    _ => null
                }
            );
        }
        catch (UniqueConstraintException e)
        {
            return e.ToValidationFailures(propertyName =>
                propertyName switch
                {
                    nameof(WorkspaceInvitation.WorkspaceId) => ("workspace_id", "User is already invited"),
                    nameof(WorkspaceInvitation.UserId) => ("user_id", "User is already invited"),
                    _ => null
                }
            );
        }

        return new Success();
    }
}
