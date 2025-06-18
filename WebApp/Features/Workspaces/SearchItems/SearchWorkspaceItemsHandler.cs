using System.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;
using WebApp.Infrastructure.Persistence.Abstractions;

namespace WebApp.Features.Workspaces.SearchItems;

public sealed class SearchWorkspaceItemsHandler(AppDbContext db)
    : ICommandHandler<SearchWorkspaceItems, PaginatedList<WorkspaceItem>>
{
    public async Task<PaginatedList<WorkspaceItem>> ExecuteAsync(SearchWorkspaceItems command, CancellationToken ct)
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

        var matchedProjects = await GetProjectSearchResultsAsync(
                command.WorkspaceId,
                command.Query,
                threshold,
                command.Size,
                ct
            )
            .ConfigureAwait(false);

        var matchedIssues = await GetIssueSearchResultsAsync(
                command.WorkspaceId,
                command.Query,
                threshold,
                command.Size,
                ct
            )
            .ConfigureAwait(false);

        var projects = await LoadProjectsAsync(matchedProjects.Keys, command.SelectProject, ct).ConfigureAwait(false);

        var issues = await LoadIssuesAsync(matchedIssues.Keys, command.SelectIssue, ct).ConfigureAwait(false);

        var items = new List<WorkspaceItem>(Math.Min(projects.Count + issues.Count, command.Size));
        foreach (var project in projects)
        {
            if (matchedProjects.TryGetValue(project.Id, out var score))
            {
                items.Add(new WorkspaceItemProject { Item = project, Score = score });
            }
        }
        foreach (var issue in issues)
        {
            if (matchedIssues.TryGetValue(issue.Id, out var score))
            {
                items.Add(new WorkspaceItemIssue { Item = issue, Score = score });
            }
        }

        await transaction.CommitAsync(ct).ConfigureAwait(false);
        return new() { Items = [.. items.OrderByDescending(a => a.Score).Take(command.Size)], TotalCount = 0 };
    }

    async Task<IDictionary<ProjectId, double>> GetProjectSearchResultsAsync(
        WorkspaceId workspaceId,
        string query,
        double threshold,
        int size,
        CancellationToken ct
    )
    {
        var projectSearchQuery = db
            .Projects.Where(a =>
                a.WorkspaceId == workspaceId
                && (
                    a.SearchVector.Matches(EF.Functions.PlainToTsQuery("simple_unaccented", query))
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Identifier),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    )
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Name),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    )
                )
            )
            .Select(a => new
            {
                a.Id,
                Score = a.SearchVector.Rank(EF.Functions.PlainToTsQuery("simple_unaccented", query)) * 4
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Identifier),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    )
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Name),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    ),
            })
            .Where(a => a.Score >= threshold);
        var searchResults = await projectSearchQuery
            .OrderByDescending(a => a.Score)
            .Take(size)
            .ToDictionaryAsync(a => a.Id, ct)
            .ConfigureAwait(false);
        return searchResults.ToDictionary(a => a.Key, a => a.Value.Score);
    }

    async Task<IDictionary<IssueId, double>> GetIssueSearchResultsAsync(
        WorkspaceId workspaceId,
        string query,
        double threshold,
        int size,
        CancellationToken ct
    )
    {
        var searchQuery = db
            .Issues.Where(a =>
                a.Project.WorkspaceId == workspaceId
                && (
                    a.SearchVector.Matches(EF.Functions.PlainToTsQuery("simple_unaccented", query))
                    || EF.Functions.TrigramsAreSimilar(
                        CustomDbFunctions.ImmutableUnaccent(a.Title),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    )
                )
            )
            .Select(a => new
            {
                a.Id,
                Score = a.SearchVector.Rank(EF.Functions.PlainToTsQuery("simple_unaccented", query)) * 4
                    + EF.Functions.TrigramsSimilarity(
                        CustomDbFunctions.ImmutableUnaccent(a.Title),
                        CustomDbFunctions.ImmutableUnaccent(query)
                    ),
            })
            .Where(a => a.Score >= threshold);
        var searchResults = await searchQuery
            .OrderByDescending(a => a.Score)
            .Take(size)
            .ToDictionaryAsync(a => a.Id, ct)
            .ConfigureAwait(false);
        return searchResults.ToDictionary(a => a.Key, a => a.Value.Score);
    }

    async Task<List<Project>> LoadProjectsAsync(ICollection<ProjectId> projectIds, string? select, CancellationToken ct)
    {
        var query = db.Projects.Where(a => projectIds.Contains(a.Id));
        if (!string.IsNullOrEmpty(select))
        {
            query = query.Select(ExpressionHelper.Select<Project, Project>(select));
        }
        var projects = await query.ToListAsync(ct).ConfigureAwait(false);
        return projects ?? [];
    }

    async Task<List<Issue>> LoadIssuesAsync(ICollection<IssueId> issueIds, string? select, CancellationToken ct)
    {
        var query = db.Issues.Where(a => issueIds.Contains(a.Id));
        if (!string.IsNullOrEmpty(select))
        {
            query = query.Select(ExpressionHelper.Select<Issue, Issue>(select));
        }
        var issues = await query.ToListAsync(ct).ConfigureAwait(false);
        return issues ?? [];
    }
}
