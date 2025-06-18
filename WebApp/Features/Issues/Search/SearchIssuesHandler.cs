using System.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

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

        var threshold = command.Threshold ?? 0.3f;
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
        await db
            .Database.ExecuteSqlRawAsync($"SET pg_trgm.similarity_threshold = {threshold};", ct)
            .ConfigureAwait(false);
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.
        var searchQuery = db
            .Issues.Where(a =>
                a.ProjectId == command.ProjectId
                && (
                    a.SearchVector.Matches(EF.Functions.PlainToTsQuery("simple_unaccented", command.Query))
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Title),
                        CustomDbFunctions.ImmutableUnaccent(command.Query)
                    )
                )
            )
            .Select(a => new
            {
                a.Id,
                Score = a.SearchVector.Rank(EF.Functions.PlainToTsQuery("simple_unaccented", command.Query))
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Title),
                        CustomDbFunctions.ImmutableUnaccent(command.Query)
                    ) * 2,
            })
            .Where(a => a.Score >= threshold);
        var totalCount = await searchQuery.CountAsync(ct).ConfigureAwait(false);
        var searchResults = await searchQuery
            .OrderByDescending(a => a.Score)
            .Take(command.Size)
            .ToDictionaryAsync(a => a.Id, ct)
            .ConfigureAwait(false);

        var ids = searchResults.Select(a => a.Value.Id).ToList();
        var query = db.Issues.Where(a => ids.Contains(a.Id));

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

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Issue, Issue>(command.Select));
        }

        query = command.Order.SortOrDefault(query);

        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        await transaction.CommitAsync(ct).ConfigureAwait(false);
        return new()
        {
            Items = command.Order.Length != 0 ? items : [.. items.OrderByDescending(a => searchResults[a.Id].Score)],
            TotalCount = totalCount,
        };
    }
}
