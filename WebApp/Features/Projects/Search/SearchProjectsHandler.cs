using System.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

namespace WebApp.Features.Projects.Search;

public sealed class SearchProjectsHandler(AppDbContext db) : ICommandHandler<SearchProjects, PaginatedList<Project>>
{
    public async Task<PaginatedList<Project>> ExecuteAsync(SearchProjects command, CancellationToken ct)
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
            .Projects.Where(a =>
                a.WorkspaceId == command.WorkspaceId
                && (
                    a.SearchVector.Matches(EF.Functions.PlainToTsQuery("simple_unaccented", command.Query))
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Identifier),
                        CustomDbFunctions.ImmutableUnaccent(command.Query)
                    )
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Name),
                        CustomDbFunctions.ImmutableUnaccent(command.Query)
                    )
                )
            )
            .Select(a => new
            {
                a.Id,
                Score = a.SearchVector.Rank(EF.Functions.PlainToTsQuery("simple_unaccented", command.Query))
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Identifier),
                        CustomDbFunctions.ImmutableUnaccent(command.Query)
                    ) * 2
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Name),
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
        var query = db.Projects.Where(a => ids.Contains(a.Id));

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Project, Project>(command.Select));
        }

        var items = await query.Skip(command.Offset).Take(command.Size).ToListAsync(ct).ConfigureAwait(false);
        await transaction.CommitAsync(ct).ConfigureAwait(false);
        return new() { Items = [.. items.OrderByDescending(a => searchResults[a.Id].Score)], TotalCount = totalCount };
    }
}
