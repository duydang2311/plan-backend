using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.GetMany;

public sealed class GetProjectMemberInvitationsHandler(AppDbContext db)
    : ICommandHandler<GetProjectMemberInvitations, PaginatedList<ProjectMemberInvitation>>
{
    public async Task<PaginatedList<ProjectMemberInvitation>> ExecuteAsync(
        GetProjectMemberInvitations command,
        CancellationToken ct
    )
    {
        var query = db.ProjectMemberInvitations.Where(a => a.ProjectId == command.ProjectId);
        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(
                ExpressionHelper.Select<ProjectMemberInvitation, ProjectMemberInvitation>(command.Select)
            );
        }

        return PaginatedList.From(
            await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false),
            totalCount
        );
    }
}
