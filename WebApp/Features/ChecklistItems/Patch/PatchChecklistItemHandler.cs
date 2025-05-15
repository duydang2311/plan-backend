using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Npgsql;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ChecklistItems.Patch;

public sealed class PatchChecklistItemHandler(AppDbContext db)
    : ICommandHandler<PatchChecklistItem, OneOf<InvalidPatchError, NotFoundError, Success>>
{
    public async Task<OneOf<InvalidPatchError, NotFoundError, Success>> ExecuteAsync(
        PatchChecklistItem command,
        CancellationToken ct
    )
    {
        Expression<Func<SetPropertyCalls<ChecklistItem>, SetPropertyCalls<ChecklistItem>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.Content, out var content) && !string.IsNullOrEmpty(content))
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Content, content));
        }
        if (command.Patch.TryGetValue(a => a.Completed, out var completed) && completed.HasValue)
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.Completed, completed));
        }

        if (updateEx is null)
        {
            return new InvalidPatchError();
        }

        var count = 0;
        try
        {
            count = await db
                .ChecklistItems.Where(a => a.Id == command.Id)
                .ExecuteUpdateAsync(updateEx, ct)
                .ConfigureAwait(false);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException postgresEx && postgresEx.SqlState.Equals("23514"))
        {
            if (postgresEx.ConstraintName is null)
            {
                throw;
            }

            if (
                postgresEx.ConstraintName.EqualsEither(
                    ["CHK_valid_todo", "CHK_valid_sub_issue"],
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                return new InvalidPatchError();
            }

            throw;
        }

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
