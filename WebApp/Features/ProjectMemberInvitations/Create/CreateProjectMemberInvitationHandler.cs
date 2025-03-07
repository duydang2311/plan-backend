using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.Create;

public sealed class CreateProjectMemberInvitationHandler(AppDbContext db)
    : ICommandHandler<
        CreateProjectMemberInvitation,
        OneOf<ValidationFailures, AlreadyIsMemberError, ConflictError, Success>
    >
{
    public async Task<OneOf<ValidationFailures, AlreadyIsMemberError, ConflictError, Success>> ExecuteAsync(
        CreateProjectMemberInvitation command,
        CancellationToken ct
    )
    {
        if (
            await db
                .ProjectMembers.AnyAsync(a => a.UserId == command.UserId && a.ProjectId == command.ProjectId, ct)
                .ConfigureAwait(false)
        )
        {
            return new AlreadyIsMemberError();
        }

        var invitation = new ProjectMemberInvitation
        {
            ProjectId = command.ProjectId,
            UserId = command.UserId,
            RoleId = command.RoleId,
        };

        db.Add(invitation);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (UniqueConstraintException)
        {
            return new ConflictError();
        }
        catch (ReferenceConstraintException e)
        {
            return e.ToValidationFailures(property =>
                property switch
                {
                    nameof(ProjectMember.ProjectId) => ("projectId", "Project does not exist"),
                    nameof(ProjectMember.UserId) => ("userId", "User does not exist"),
                    _ => (property, $"{property} is invalid"),
                }
            );
        }

        await new ProjectMemberInvited { ProjectMemberInvitation = invitation }
            .PublishAsync(Mode.WaitForAll, ct)
            .ConfigureAwait(false);

        return new Success();
    }
}
