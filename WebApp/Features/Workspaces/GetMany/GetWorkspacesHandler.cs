using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Workspaces.GetMany;

public sealed class GetWorkspacesHandler(AppDbContext dbContext)
    : ICommandHandler<GetWorkspaces, PaginatedList<Workspace>>
{
    public async Task<PaginatedList<Workspace>> ExecuteAsync(GetWorkspaces command, CancellationToken ct)
    {
        var query = dbContext.Workspaces.AsQueryable();
        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.Id == command.WorkspaceId.Value);
        }
        if (command.UserId.HasValue)
        {
            query = query.Where(a => a.Users.Any(b => b.Id == command.UserId.Value));
        }
        if (command.Path is not null)
        {
            query = query.Where(a => a.Path.Equals(command.Path));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Workspace, Workspace>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
