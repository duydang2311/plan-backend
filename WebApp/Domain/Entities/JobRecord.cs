using System.Text.Json;
using FastEndpoints;

namespace WebApp.Domain.Entities;

public sealed record class JobRecord : IJobStorageRecord, IJobResultStorage
{
    public Guid TrackingID { get; set; }
    public string QueueID { get; set; } = default!;
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }
    public string CommandJson { get; set; } = default!;
    public object Command { get; set; } = default!;
    public object? Result { get; set; }
    public string? ResultJson { get; set; }

    TCommand IJobStorageRecord.GetCommand<TCommand>() => JsonSerializer.Deserialize<TCommand>(CommandJson)!;

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command) => CommandJson = JsonSerializer.Serialize(command);

    TResult? IJobResultStorage.GetResult<TResult>()
        where TResult : default => ResultJson is not null ? JsonSerializer.Deserialize<TResult>(ResultJson) : default;

    void IJobResultStorage.SetResult<TResult>(TResult result) => ResultJson = JsonSerializer.Serialize(result);
}
