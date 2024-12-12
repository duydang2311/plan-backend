using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.Search;

public sealed class SearchUsersHandler(AppDbContext db) : ICommandHandler<SearchUsers, PaginatedList<User>>
{
    public async Task<PaginatedList<User>> ExecuteAsync(SearchUsers command, CancellationToken ct)
    {
        await using var transaction = (
            await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct).ConfigureAwait(false)
        );

        await db.Database.ExecuteSqlRawAsync("SET pg_trgm.similarity_threshold = 0.1", ct).ConfigureAwait(false);

        var query = db.Users.Where(a => EF.Functions.TrigramsAreSimilar(a.Email, command.Query));

        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.Workspaces.Any(b => b.Id == command.WorkspaceId));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var parameter = Expression.Parameter(typeof(User), "e");
        var construct = Expression.New(
            typeof(UserWithSimilarity).GetConstructor([typeof(User), typeof(double)])!,
            [
                string.IsNullOrEmpty(command.Select)
                    ? parameter
                    : ExpressionHelper.Init<User, User>(parameter, command.Select),
                TrigramsSimilarity(
                    Expression.Property(parameter, nameof(User.Email)),
                    Expression.Property(Expression.Constant(command), nameof(SearchUsers.Query))
                )
            ],
            typeof(UserWithSimilarity).GetProperties()
        );
        var param2 = Expression.Parameter(typeof(UserWithSimilarity), "e");
        var select = Expression.Lambda<Func<User, UserWithSimilarity>>(construct, parameter);
        var orderBy = Expression.Lambda<Func<UserWithSimilarity, double>>(
            Expression.Property(param2, "Similarity"),
            param2
        );
        var items = await query
            .Select(select)
            .OrderByDescending(orderBy)
            .Take(command.Size)
            .Skip(command.Offset)
            .ToArrayAsync(ct)
            .ConfigureAwait(false);
        await transaction.CommitAsync(ct).ConfigureAwait(false);
        return new() { Items = [.. items.Select(a => a.User)], TotalCount = totalCount };
    }

    private static MethodCallExpression TrigramsSimilarity(Expression arg1, Expression arg2)
    {
        var methodInfo = typeof(NpgsqlTrigramsDbFunctionsExtensions).GetMethod(
            "TrigramsSimilarity",
            BindingFlags.Static | BindingFlags.Public
        )!;
        var functions = Expression.MakeMemberAccess(null, typeof(EF).GetProperty("Functions")!);
        var call = Expression.Call(null, methodInfo, [functions, arg1, arg2]);
        return call;
    }

    public sealed class UserWithSimilarity(User user, double similarity)
    {
        public User User { get; init; } = user;
        public double Similarity { get; init; } = similarity;
    }
}
