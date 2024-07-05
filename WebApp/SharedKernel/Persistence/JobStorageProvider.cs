using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Persistence;

public sealed record class JobStorageProvider(IDbContextFactory<AppDbContext> dbContextFactory)
    : IJobStorageProvider<JobRecord>
{
    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
        await dbContext.JobRecords.Where(x => x.TrackingID == trackingId).ExecuteDeleteAsync(ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> parameters)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await dbContext
            .JobRecords.Where(parameters.Match)
            .Take(parameters.Limit)
            .ToArrayAsync(parameters.CancellationToken)
            .ConfigureAwait(false);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord r, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
        await dbContext
            .JobRecords.Where(x => x.QueueID == r.QueueID)
            .ExecuteUpdateAsync(c => c.SetProperty(x => x.IsComplete, true), ct)
            .ConfigureAwait(false);
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord r, Exception exception, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
        await dbContext
            .JobRecords.Where(x => x.QueueID == r.QueueID)
            .ExecuteUpdateAsync(c => c.SetProperty(x => x.ExecuteAfter, DateTime.UtcNow.AddMinutes(1)), ct)
            .ConfigureAwait(false);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> parameters)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
        await dbContext.JobRecords.Where(parameters.Match).ExecuteDeleteAsync().ConfigureAwait(false);
    }

    public async Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
        dbContext.Add(r);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
