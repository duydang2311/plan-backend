using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceStatuses.Patch;

using Result = OneOf<ValidationFailures, NotFoundError, Success>;

public sealed class PatchWorkspaceStatusHandler(AppDbContext db) : ICommandHandler<PatchWorkspaceStatus, Result>
{
    public async Task<Result> ExecuteAsync(PatchWorkspaceStatus command, CancellationToken ct)
    {
        var query = db.WorkspaceStatuses.Where(a => a.Id == command.StatusId).AsQueryable();

        Expression<Func<SetPropertyCalls<WorkspaceStatus>, SetPropertyCalls<WorkspaceStatus>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.Rank, out var rank))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Rank, rank));
        }

        if (updateEx is null)
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        var count = await db
            .WorkspaceStatuses.Where(a => a.Id == command.StatusId)
            .ExecuteUpdateAsync(updateEx, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }
        return new Success();
    }
}
