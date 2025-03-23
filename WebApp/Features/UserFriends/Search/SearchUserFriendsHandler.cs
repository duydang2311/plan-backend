using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserFriends.Search;

public sealed class SearchUsersHandler(AppDbContext db) : ICommandHandler<SearchUserFriends, PaginatedList<UserFriend>>
{
    public async Task<PaginatedList<UserFriend>> ExecuteAsync(SearchUserFriends command, CancellationToken ct)
    {
        await using var transaction = await db
            .Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct)
            .ConfigureAwait(false);

        await db.Database.ExecuteSqlRawAsync("SET pg_trgm.similarity_threshold = 0.2", ct).ConfigureAwait(false);

        var query = db
            .UserFriends.AsSplitQuery()
            .Where(a =>
                a.UserId == command.UserId
                && (
                    EF.Functions.TrigramsAreSimilar(a.Friend.Trigrams, command.Query)
                    || (
                        a.Friend.Profile != null
                        && EF.Functions.TrigramsAreSimilar(a.Friend.Profile.Trigrams, command.Query)
                    )
                )
            )
            .Concat(
                db.UserFriends.Where(a =>
                    a.FriendId == command.UserId
                    && (
                        EF.Functions.TrigramsAreSimilar(a.User.Trigrams, command.Query)
                        || (
                            a.User.Profile != null
                            && EF.Functions.TrigramsAreSimilar(a.User.Profile.Trigrams, command.Query)
                        )
                    )
                )
            );

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var parameter = Expression.Parameter(typeof(UserFriend), "e");
        var construct = Expression.New(
            typeof(UserFriendWithSimilarity).GetConstructor([typeof(UserFriend), typeof(double)])!,
            [
                string.IsNullOrEmpty(command.Select)
                    ? parameter
                    : ExpressionHelper.Init<UserFriend, UserFriend>(parameter, command.Select),
                Expression.Condition(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(UserFriend.UserId)),
                        Expression.Constant(command.UserId)
                    ),
                    Expression.Add(
                        TrigramsSimilarity(
                            Expression.Property(
                                Expression.Property(parameter, nameof(UserFriend.Friend)),
                                nameof(User.Trigrams)
                            ),
                            Expression.Property(Expression.Constant(command), nameof(SearchUserFriends.Query))
                        ),
                        Expression.Condition(
                            Expression.Equal(
                                Expression.Property(
                                    Expression.Property(parameter, nameof(UserFriend.Friend)),
                                    nameof(User.Profile)
                                ),
                                Expression.Constant(null, typeof(UserProfile))
                            ),
                            Expression.Constant(0.0, typeof(double)),
                            TrigramsSimilarity(
                                Expression.Property(
                                    Expression.Property(
                                        Expression.Property(parameter, nameof(UserFriend.Friend)),
                                        nameof(User.Profile)
                                    ),
                                    nameof(UserProfile.Trigrams)
                                ),
                                Expression.Property(Expression.Constant(command), nameof(SearchUserFriends.Query))
                            )
                        )
                    ),
                    Expression.Add(
                        TrigramsSimilarity(
                            Expression.Property(
                                Expression.Property(parameter, nameof(UserFriend.User)),
                                nameof(User.Trigrams)
                            ),
                            Expression.Property(Expression.Constant(command), nameof(SearchUserFriends.Query))
                        ),
                        Expression.Condition(
                            Expression.Equal(
                                Expression.Property(
                                    Expression.Property(parameter, nameof(UserFriend.User)),
                                    nameof(User.Profile)
                                ),
                                Expression.Constant(null, typeof(UserProfile))
                            ),
                            Expression.Constant(0.0, typeof(double)),
                            TrigramsSimilarity(
                                Expression.Property(
                                    Expression.Property(
                                        Expression.Property(parameter, nameof(UserFriend.User)),
                                        nameof(User.Profile)
                                    ),
                                    nameof(UserProfile.Trigrams)
                                ),
                                Expression.Property(Expression.Constant(command), nameof(SearchUserFriends.Query))
                            )
                        )
                    )
                ),
            ],
            typeof(UserFriendWithSimilarity).GetProperties()
        );
        var param2 = Expression.Parameter(typeof(UserFriendWithSimilarity), "e");
        var select = Expression.Lambda<Func<UserFriend, UserFriendWithSimilarity>>(construct, parameter);
        var orderBy = Expression.Lambda<Func<UserFriendWithSimilarity, double>>(
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
        return new() { Items = [.. items.Select(a => a.UserFriend)], TotalCount = totalCount };
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

    public sealed class UserFriendWithSimilarity(UserFriend userFriend, double similarity)
    {
        public UserFriend UserFriend { get; init; } = userFriend;
        public double Similarity { get; init; } = similarity;
    }
}
