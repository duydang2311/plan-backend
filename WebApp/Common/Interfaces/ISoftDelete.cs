using NodaTime;

namespace WebApp.Common.Interfaces;

public interface ISoftDelete
{
    public Instant? DeletedTime { get; init; }
}
