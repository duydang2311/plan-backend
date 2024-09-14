using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectStatuses.GetMany;

using Result = OneOf<PaginatedList<ProjectStatus>>;

public sealed record GetWorkspaceStatusesHandler(AppDbContext db) : ICommandHandler<GetProjectStatuses, Result>
{
    public async Task<Result> ExecuteAsync(GetProjectStatuses command, CancellationToken ct)
    {
        var query = db.ProjectStatuses.Where(a => a.ProjectId == command.ProjectId).AsQueryable();

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ProjectStatus, ProjectStatus>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        query = command
            .Order.Where(static x => x.Name.EqualsEither(["Status.Rank"], StringComparison.OrdinalIgnoreCase))
            .SortOrDefault(query);
        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        return new PaginatedList<ProjectStatus> { TotalCount = totalCount, Items = items };
    }
}
