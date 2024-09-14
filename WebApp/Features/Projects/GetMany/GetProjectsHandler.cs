using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.GetMany;

using Result = OneOf<PaginatedList<Project>>;

public sealed class GetProjectsHandler(AppDbContext db) : ICommandHandler<GetProjects, Result>
{
    public async Task<Result> ExecuteAsync(GetProjects command, CancellationToken ct)
    {
        var query = db.Projects.AsQueryable();

        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.WorkspaceId == command.WorkspaceId);
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Project, Project>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command
            .Order.Where(static x =>
                x.Name.EqualsEither(
                    [
                        nameof(Project.CreatedTime),
                        nameof(Project.UpdatedTime),
                        nameof(Project.Name),
                        nameof(Project.Identifier),
                    ],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .SortOrDefault(query);
        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        return new PaginatedList<Project> { TotalCount = totalCount, Items = items };
    }
}
