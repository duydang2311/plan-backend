using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.Create;

public sealed class CreateProjectMemberInvitationHandler(AppDbContext db)
    : ICommandHandler<CreateProjectMemberInvitation, OneOf<ValidationFailures, ConflictError, Success>>
{
    public async Task<OneOf<ValidationFailures, ConflictError, Success>> ExecuteAsync(
        CreateProjectMemberInvitation command,
        CancellationToken ct
    )
    {
        var invitation = new ProjectMemberInvitation
        {
            ProjectId = command.ProjectId,
            UserId = command.UserId,
            RoleId = command.RoleId
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
                    _ => (property, $"{property} is invalid")
                }
            );
        }
        return new Success();
    }
}
