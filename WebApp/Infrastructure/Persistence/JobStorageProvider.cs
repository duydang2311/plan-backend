using System.Text.Json;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;

namespace WebApp.Infrastructure.Persistence;

public sealed record class JobStorageProvider(IDbContextFactory<AppDbContext> dbContextFactory)
    : IJobStorageProvider<JobRecord>,
        IJobResultProvider
{
    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        await db.JobRecords.Where(x => x.TrackingID == trackingId).ExecuteDeleteAsync(ct).ConfigureAwait(false);
    }

    public async Task<TResult?> GetJobResultAsync<TResult>(Guid trackingId, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        var job = await db.JobRecords.FirstOrDefaultAsync(j => j.TrackingID == trackingId, ct).ConfigureAwait(false);

        return job is not null ? ((IJobResultStorage)job).GetResult<TResult>() : default;
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> parameters)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
#pragma warning restore CA2007
        return await db
            .JobRecords.Where(parameters.Match)
            .OrderBy(a => a.ExecuteAfter)
            .Take(parameters.Limit)
            .ToArrayAsync(parameters.CancellationToken)
            .ConfigureAwait(false);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord r, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        await db
            .JobRecords.Where(x => x.QueueID == r.QueueID)
            .ExecuteUpdateAsync(c => c.SetProperty(x => x.IsComplete, true), ct)
            .ConfigureAwait(false);
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord r, Exception exception, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        await db
            .JobRecords.Where(x => x.QueueID == r.QueueID)
            .ExecuteUpdateAsync(c => c.SetProperty(x => x.ExecuteAfter, DateTime.UtcNow.AddMinutes(1)), ct)
            .ConfigureAwait(false);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> parameters)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync().ConfigureAwait(false);
#pragma warning restore CA2007
        await db.JobRecords.Where(parameters.Match).ExecuteDeleteAsync().ConfigureAwait(false);
    }

    public async Task StoreJobAsync(JobRecord r, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        db.Add(r);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    public async Task StoreJobResultAsync<TResult>(Guid trackingId, TResult result, CancellationToken ct)
    {
#pragma warning disable CA2007
        await using var db = await dbContextFactory.CreateDbContextAsync(ct).ConfigureAwait(false);
#pragma warning restore CA2007
        var json = JsonSerializer.Serialize(result);
        await db
            .JobRecords.Where(a => a.TrackingID == trackingId)
            .ExecuteUpdateAsync(a => a.SetProperty(b => b.ResultJson, json), ct)
            .ConfigureAwait(false);
    }
}
