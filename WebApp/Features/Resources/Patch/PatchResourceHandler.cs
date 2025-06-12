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

namespace WebApp.Features.Resources.Patch;

public sealed class PatchResourceHandler(AppDbContext db)
    : ICommandHandler<PatchResource, OneOf<NotFoundError, Success>>
{
    public async Task<OneOf<NotFoundError, Success>> ExecuteAsync(PatchResource command, CancellationToken ct)
    {
        var count = 0;
        var query = db.Resources.Where(a => a.Id == command.Id);
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var transaction = await db.Database.BeginTransactionAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

        Expression<Func<SetPropertyCalls<Resource>, SetPropertyCalls<Resource>>>? expression = default;
        if (command.Patch.TryGetValue(a => a.Name, out var name))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(b => b.Name, name));
        }
        if (command.Patch.TryGetValue(a => a.DocumentContent, out var document))
        {
            count = await db
                .Database.ExecuteSqlAsync(
                    $"""
                    INSERT INTO resource_documents (resource_id, content)
                    VALUES ({command.Id.Value}, {document})
                    ON CONFLICT (resource_id) DO UPDATE SET content = EXCLUDED.content;
                    """,
                    ct
                )
                .ConfigureAwait(false);
        }

        if (expression is null)
        {
            await transaction.CommitAsync(ct).ConfigureAwait(false);
            return new Success();
        }

        count = Math.Max(count, await query.ExecuteUpdateAsync(expression, ct).ConfigureAwait(false));

        if (count == 0)
        {
            return new NotFoundError();
        }

        await transaction.CommitAsync(ct).ConfigureAwait(false);
        return new Success();
    }
}
