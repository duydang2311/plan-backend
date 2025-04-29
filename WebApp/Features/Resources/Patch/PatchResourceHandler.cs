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
        var query = db.Resources.Where(a => a.Id == command.Id);

        Expression<Func<SetPropertyCalls<Resource>, SetPropertyCalls<Resource>>>? expression = default;
        if (command.Patch.TryGetValue(a => a.Name, out var name))
        {
            expression = ExpressionHelper.Append(expression, a => a.SetProperty(b => b.Name, name));
        }

        if (expression is null)
        {
            return new Success();
        }

        var count = await query.ExecuteUpdateAsync(expression, ct).ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
