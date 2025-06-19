using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.Create;

public sealed class CreateProjectMemberHandler(AppDbContext db)
    : ICommandHandler<CreateProjectMember, OneOf<ValidationFailures, ConflictError, Success>>
{
    public async Task<OneOf<ValidationFailures, ConflictError, Success>> ExecuteAsync(
        CreateProjectMember command,
        CancellationToken ct
    )
    {
        var projectMember = new ProjectMember
        {
            ProjectId = command.ProjectId,
            UserId = command.UserId,
            RoleId = command.RoleId,
        };

        db.Add(projectMember);
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
        return new Success();
    }
}
