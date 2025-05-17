using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.Search;

public sealed class SearchIssuesHandler(AppDbContext db) : ICommandHandler<SearchIssues, PaginatedList<Issue>>
{
    public async Task<PaginatedList<Issue>> ExecuteAsync(SearchIssues command, CancellationToken ct)
    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using var transaction = await db
            .Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct)
            .ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

        await db.Database.ExecuteSqlRawAsync("SET pg_trgm.similarity_threshold = 0.05", ct).ConfigureAwait(false);

        var query = db.Issues.Where(a =>
            a.ProjectId == command.ProjectId && EF.Functions.TrigramsAreSimilar(a.Trigrams, command.Query)
        );

        if (command.ExcludeIssueIds is not null)
        {
            query = query.Where(a => !command.ExcludeIssueIds.Contains(a.Id));
        }

        if (command.ExcludeChecklistItemParentIssueId.HasValue)
        {
            query = query.Where(a =>
                a.ParentChecklistItems.All(b => b.ParentIssueId != command.ExcludeChecklistItemParentIssueId)
            );
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var parameter = Expression.Parameter(typeof(Issue), "a");
        var construct = Expression.New(
            typeof(IssueWithSimilarity).GetConstructor([typeof(Issue), typeof(double)])!,
            [
                string.IsNullOrEmpty(command.Select)
                    ? parameter
                    : ExpressionHelper.Init<Issue, Issue>(parameter, command.Select),
                TrigramsSimilarity(
                    Expression.Property(parameter, nameof(Issue.Trigrams)),
                    Expression.Property(Expression.Constant(command), nameof(SearchIssues.Query))
                ),
            ],
            typeof(IssueWithSimilarity).GetProperties()
        );
        var param2 = Expression.Parameter(typeof(IssueWithSimilarity), "b");
        var select = Expression.Lambda<Func<Issue, IssueWithSimilarity>>(construct, parameter);
        var orderBy = Expression.Lambda<Func<IssueWithSimilarity, double>>(
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
        return new() { Items = [.. items.Select(a => a.Issue)], TotalCount = totalCount };
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

    public sealed class IssueWithSimilarity(Issue issue, double similarity)
    {
        public Issue Issue { get; init; } = issue;
        public double Similarity { get; init; } = similarity;
    }
}
