using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly struct UserNotificationId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly ProjectId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool operator >(UserNotificationId a, UserNotificationId b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(UserNotificationId a, UserNotificationId b)
    {
        return a.Value < b.Value;
    }
}
