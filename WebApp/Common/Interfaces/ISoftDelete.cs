using NodaTime;

namespace WebApp.Common.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; init; }
    public Instant? DeletedTime { get; init; }
}
