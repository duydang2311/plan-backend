using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.Accept;

public sealed record AcceptProjectMemberInvitationHandler(AppDbContext db)
    : ICommandHandler<AcceptProjectMemberInvitation, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(
        AcceptProjectMemberInvitation command,
        CancellationToken ct
    )
    {
        var invitation = await db
            .ProjectMemberInvitations.Where(a => a.Id == command.ProjectMemberInvitationId)
            .Select(a => new
            {
                a.ProjectId,
                a.UserId,
                a.RoleId,
            })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (invitation is null)
        {
            return new NotFoundError();
        }

        await db
            .ProjectMemberInvitations.Where(a => a.Id == command.ProjectMemberInvitationId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        db.Add(
            new ProjectMember
            {
                ProjectId = invitation.ProjectId,
                UserId = invitation.UserId,
                RoleId = invitation.RoleId,
            }
        );
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return new Success();
    }
}
