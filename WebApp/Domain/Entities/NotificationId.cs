using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly struct NotificationId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly ProjectId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
