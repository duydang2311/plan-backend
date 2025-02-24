using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.Create;

using Result = OneOf<ConflictError, ValidationFailures, Project>;

public sealed class CreateProjectHandler(AppDbContext db) : ICommandHandler<CreateProject, Result>
{
    public async Task<Result> ExecuteAsync(CreateProject command, CancellationToken ct)
    {
        var project = new Project
        {
            Id = IdHelper.NewProjectId(),
            WorkspaceId = command.WorkspaceId,
            Name = command.Name,
            Description = command.Description,
            Identifier = command.Identifier,
            Members = [new ProjectMember { UserId = command.UserId, RoleId = ProjectRoleDefaults.Admin.Id }],
        };

        db.Add(new SharedCounter { Id = project.Id.Value });
        db.Add(project);

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
            if (e.ConstraintProperties.Any(a => a.Equals("workspace_id")))
            {
                return ValidationFailures.Single("workspaceId", "Workspace does not exist", "no_reference");
            }
            if (e.ConstraintProperties.Any(a => a.Equals("user_id")))
            {
                return ValidationFailures.Single("userId", "User does not exist", "no_reference");
            }
            throw;
        }

        await new ProjectCreated { Project = project }
            .PublishAsync(Mode.WaitForAll, ct)
            .ConfigureAwait(false);

        return project;
    }
}
