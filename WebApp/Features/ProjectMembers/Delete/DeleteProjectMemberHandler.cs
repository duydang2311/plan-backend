using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.Delete;

public sealed class DeleteProjectMemberHandler(AppDbContext db)
    : ICommandHandler<DeleteProjectMember, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(DeleteProjectMember command, CancellationToken ct)
    {
        var count = await db.ProjectMembers.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct).ConfigureAwait(false);
        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
