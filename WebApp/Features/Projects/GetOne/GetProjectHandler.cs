using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.GetOne;

public sealed class GetProjectHandler(AppDbContext db)
    : ICommandHandler<GetProject, OneOf<ValidationFailures, None, Project>>
{
    public async Task<OneOf<ValidationFailures, None, Project>> ExecuteAsync(GetProject command, CancellationToken ct)
    {
        if (command.ProjectId is null && string.IsNullOrEmpty(command.Identifier))
        {
            return ValidationFailures
                .Many(2)
                .Add("projectId", "require fields projectId or identifier", "required_either")
                .Add("identifier", "require fields projectId or identifier", "required_either");
        }

        var query = db.Projects.AsQueryable();

        if (command.ProjectId is not null)
        {
            query = query.Where(a => a.Id == command.ProjectId);
        }

        if (!string.IsNullOrEmpty(command.Identifier))
        {
            query = query.Where(a => a.Identifier.Equals(command.Identifier));
        }

        var project = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return project is null ? new None() : project;
    }
}
