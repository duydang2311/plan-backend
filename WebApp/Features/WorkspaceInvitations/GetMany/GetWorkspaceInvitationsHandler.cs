using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceInvitations.GetMany;

public sealed record GetWorkspaceInvitationsHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceInvitations, PaginatedList<WorkspaceInvitation>>
{
    public async Task<PaginatedList<WorkspaceInvitation>> ExecuteAsync(
        GetWorkspaceInvitations command,
        CancellationToken ct
    )
    {
        var query = db.WorkspaceInvitations.AsQueryable();
        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.WorkspaceId == command.WorkspaceId.Value);
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceInvitation, WorkspaceInvitation>(command.Select));
        }

        query = command
            .Order.Where(a =>
                a.Name.EqualsEither([nameof(WorkspaceInvitation.CreatedTime)], StringComparison.OrdinalIgnoreCase)
            )
            .SortOrDefault(query, query => query.OrderByDescending(a => a.CreatedTime));

        return PaginatedList.From(await query.ToListAsync(ct).ConfigureAwait(false), totalCount);
    }
}
