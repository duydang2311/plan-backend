using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Roles.GetMany;

public sealed class GetRolesHandler(AppDbContext db) : ICommandHandler<GetRoles, PaginatedList<Role>>
{
    public async Task<PaginatedList<Role>> ExecuteAsync(GetRoles command, CancellationToken ct)
    {
        var query = db.Roles.AsQueryable();

        if (string.Equals(command.Type, "workspace", StringComparison.OrdinalIgnoreCase))
        {
            var roleIds = WorkspaceRoleDefaults.Roles.Select(a => a.Id);
            query = query.Where(a => roleIds.Contains(a.Id));
        }
        else if (string.Equals(command.Type, "project", StringComparison.OrdinalIgnoreCase))
        {
            var roleIds = ProjectRoleDefaults.Roles.Select(a => a.Id);
            query = query.Where(a => roleIds.Contains(a.Id));
        }

        var countAsync = query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Role, Role>(command.Select));
        }

        query = command.Order.SortOrDefault(query, a => a.OrderBy(b => b.Id));

        var totalCount = await countAsync;

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
