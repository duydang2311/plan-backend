using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Projects.Delete;

using Result = OneOf<NotFoundError, Success>;

public sealed class DeleteProjectHandler(AppDbContext db) : ICommandHandler<DeleteProject, Result>
{
    public async Task<Result> ExecuteAsync(DeleteProject command, CancellationToken ct)
    {
        var count = await db
            .Projects.Where(a => a.Id == command.ProjectId)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
