using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.Create;

using Result = OneOf<ConflictError, ValidationFailures, Project>;

public sealed class CreateProjectHandler(AppDbContext db) : ICommandHandler<CreateProject, Result>
{
    public async Task<Result> ExecuteAsync(CreateProject command, CancellationToken ct)
    {
        if (
            await db
                .Projects.AnyAsync(
                    a => a.WorkspaceId == command.WorkspaceId && a.Identifier.Equals(command.Identifier),
                    ct
                )
                .ConfigureAwait(false)
        )
        {
            return new ConflictError();
        }

        var project = new Project
        {
            WorkspaceId = command.WorkspaceId,
            Name = command.Name,
            Description = command.Description,
            Identifier = command.Identifier
        };

        db.Add(project);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException)
        {
            return ValidationFailures.Single("workspaceId", "Workspace does not exist", "no_reference");
        }
        return project;
    }
}
