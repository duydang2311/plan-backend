using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMemberInvitations.Delete;

public sealed class DeleteProjectMemberInvitationHandler(AppDbContext db)
    : ICommandHandler<DeleteProjectMemberInvitation, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(
        DeleteProjectMemberInvitation command,
        CancellationToken ct
    )
    {
        var count = await db
            .ProjectMemberInvitations.Where(a => a.Id == command.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
