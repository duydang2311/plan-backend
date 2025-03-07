using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.GetOne;

public sealed class GetProjectMemberInvitationHandler(AppDbContext db)
    : ICommandHandler<GetProjectMemberInvitation, OneOf<NotFoundError, ProjectMemberInvitation>>
{
    public async Task<OneOf<NotFoundError, ProjectMemberInvitation>> ExecuteAsync(
        GetProjectMemberInvitation command,
        CancellationToken ct
    )
    {
        var query = db.ProjectMemberInvitations.Where(a => a.Id == command.ProjectMemberInvitationId);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(
                ExpressionHelper.Select<ProjectMemberInvitation, ProjectMemberInvitation>(command.Select)
            );
        }

        var invitation = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (invitation == null)
        {
            return new NotFoundError();
        }
        return invitation;
    }
}
