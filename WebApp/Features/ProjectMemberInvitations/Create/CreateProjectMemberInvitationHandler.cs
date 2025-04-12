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

namespace WebApp.Features.ProjectMemberInvitations.Create;

public sealed class CreateProjectMemberInvitationHandler(
    AppDbContext db,
    IDbContextOutbox outbox,
    ILogger<CreateProjectMemberInvitationHandler> logger
)
    : ICommandHandler<
        CreateProjectMemberInvitation,
        OneOf<ValidationFailures, AlreadyIsMemberError, ConflictError, ServerError, Success>
    >
{
    public async Task<
        OneOf<ValidationFailures, AlreadyIsMemberError, ConflictError, ServerError, Success>
    > ExecuteAsync(CreateProjectMemberInvitation command, CancellationToken ct)
    {
        if (
            await db
                .ProjectMembers.AnyAsync(a => a.UserId == command.UserId && a.ProjectId == command.ProjectId, ct)
                .ConfigureAwait(false)
        )
        {
            return new AlreadyIsMemberError();
        }

        using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
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

        try
        {
            outbox.Enroll(db);
            await outbox
                .PublishAsync(
                    new ProjectMemberInvited { ProjectMemberInvitationId = invitation.Id, MemberId = invitation.UserId }
                )
                .ConfigureAwait(false);
            await outbox.SaveChangesAndFlushMessagesAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish ProjectMemberInvited message");
            return Errors.Outbox();
        }

        return new Success();
    }
}
