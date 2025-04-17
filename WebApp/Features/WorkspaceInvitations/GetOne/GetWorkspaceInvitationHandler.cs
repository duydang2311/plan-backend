using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceInvitations.GetOne;

public sealed record GetWorkspaceInvitationsHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceInvitation, OneOf<NotFoundError, WorkspaceInvitation>>
{
    public async Task<OneOf<NotFoundError, WorkspaceInvitation>> ExecuteAsync(
        GetWorkspaceInvitation command,
        CancellationToken ct
    )
    {
        var query = db.WorkspaceInvitations.Where(a => a.Id == command.Id);

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceInvitation, WorkspaceInvitation>(command.Select));
        }

        var invitation = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (invitation is null)
        {
            return new NotFoundError();
        }
        return invitation;
    }
}
