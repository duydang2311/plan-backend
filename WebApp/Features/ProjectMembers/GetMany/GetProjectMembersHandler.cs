using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.GetMany;

public sealed class GetProjectMembersHandler(AppDbContext db)
    : ICommandHandler<GetProjectMembers, PaginatedList<ProjectMember>>
{
    public async Task<PaginatedList<ProjectMember>> ExecuteAsync(GetProjectMembers command, CancellationToken ct)
    {
        var query = db.ProjectMembers.Where(a => a.ProjectId == command.ProjectId);
        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<ProjectMember, ProjectMember>(command.Select));
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
