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
        await using var transaction = await db
            .Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct)
            .ConfigureAwait(false);

        await db.Database.ExecuteSqlRawAsync("SET pg_trgm.similarity_threshold = 0.2", ct).ConfigureAwait(false);

        var query = db.Users.Where(a =>
            EF.Functions.TrigramsAreSimilar(a.Trigrams, command.Query)
            || (a.Profile != null && EF.Functions.TrigramsAreSimilar(a.Profile.Trigrams, command.Query))
        );

        if (command.WorkspaceId.HasValue)
        {
            query = query.Where(a => a.Workspaces.Any(b => b.Id == command.WorkspaceId));
        }

        if (command.ExcludeFriendsWithUserId.HasValue)
        {
            query = query.Where(a =>
                a.Id != command.ExcludeFriendsWithUserId
                && !a.UserFriends.Any(b =>
                    b.UserId == command.ExcludeFriendsWithUserId.Value
                    || b.FriendId == command.ExcludeFriendsWithUserId.Value
                )
            );
        }

        if (command.ExcludeFriendRequestedWithUserId.HasValue)
        {
            query = query.Where(a =>
                a.Id != command.ExcludeFriendRequestedWithUserId
                && !a.UserSentFriendRequests.Any(b => b.ReceiverId == command.ExcludeFriendRequestedWithUserId.Value)
                && !a.UserReceivedFriendRequests.Any(b => b.SenderId == command.ExcludeFriendRequestedWithUserId.Value)
            );
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var parameter = Expression.Parameter(typeof(User), "e");
        var construct = Expression.New(
            typeof(UserWithSimilarity).GetConstructor([typeof(User), typeof(double)])!,
            [
                string.IsNullOrEmpty(command.Select)
                    ? parameter
                    : ExpressionHelper.Init<User, User>(parameter, command.Select),
                Expression.Add(
                    TrigramsSimilarity(
                        Expression.Property(parameter, nameof(User.Trigrams)),
                        Expression.Property(Expression.Constant(command), nameof(SearchUsers.Query))
                    ),
                    Expression.Condition(
                        Expression.Equal(
                            Expression.Property(parameter, nameof(User.Profile)),
                            Expression.Constant(null, typeof(UserProfile))
                        ),
                        Expression.Constant(0.0, typeof(double)),
                        TrigramsSimilarity(
                            Expression.Property(
                                Expression.Property(parameter, nameof(User.Profile)),
                                nameof(UserProfile.Trigrams)
                            ),
                            Expression.Property(Expression.Constant(command), nameof(SearchUsers.Query))
                        )
                    )
                ),
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
