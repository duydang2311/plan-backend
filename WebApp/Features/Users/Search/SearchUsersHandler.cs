using System.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.Search;

public sealed class SearchUsersHandler(AppDbContext db) : ICommandHandler<SearchUsers, SearchUsersResult>
{
    public async Task<SearchUsersResult> ExecuteAsync(SearchUsers command, CancellationToken ct)
    {
        await using var transaction = (
            await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct).ConfigureAwait(false)
        );

        try
        {
            await db.Database.ExecuteSqlRawAsync("SET pg_trgm.similarity_threshold = 0.1", ct).ConfigureAwait(false);
            var query = db
                .Users.Where(x => EF.Functions.TrigramsAreSimilar(x.Email, command.Query))
                .Select(x => new SearchUsersResult.Item
                {
                    UserId = x.Id,
                    Email = x.Email,
                    Similarity = EF.Functions.TrigramsSimilarity(x.Email, command.Query)
                })
                .OrderByDescending(x => x.Similarity);

            var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
            var items = await query.ToArrayAsync(ct).ConfigureAwait(false);
            return new() { Items = items, TotalCount = totalCount };
        }
        finally
        {
            await transaction.CommitAsync(ct).ConfigureAwait(false);
        }
    }
}
