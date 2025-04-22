using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record StoragePendingUpload
{
    public Instant CreatedTime { get; init; }
    public StoragePendingUploadId Id { get; init; }
    public string Key { get; init; } = null!;
    public Instant ExpiryTime { get; init; }
}
