using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.GetOne;

public sealed class GetProjectHandler(AppDbContext db)
    : ICommandHandler<GetProject, OneOf<ValidationFailures, None, Project>>
{
    public async Task<OneOf<ValidationFailures, None, Project>> ExecuteAsync(GetProject command, CancellationToken ct)
    {
        var query = db.Projects.AsQueryable();

        if (command.ProjectId is not null)
        {
            query = query.Where(a => a.Id == command.ProjectId);
        }
        else if (command.WorkspaceId.HasValue && !string.IsNullOrEmpty(command.Identifier))
        {
            query = query.Where(a => a.WorkspaceId == command.WorkspaceId && a.Identifier.Equals(command.Identifier));
        }
        else
        {
            return ValidationFailures
                .Many(3)
                .Add(
                    "projectId",
                    "require fields \"projectId\" or \"workspaceId\" and \"identifier\"",
                    "required_either"
                )
                .Add(
                    "workspaceId",
                    "require fields \"projectId\" or \"workspaceId\" and \"identifier\"",
                    "required_either"
                )
                .Add(
                    "identifier",
                    "require fields \"projectId\" or \"workspaceId\" and \"identifier\"",
                    "required_either"
                );
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Project, Project>(command.Select));
        }

        var project = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return project is null ? new None() : project;
    }
}
