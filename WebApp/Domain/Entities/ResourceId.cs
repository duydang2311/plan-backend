using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly struct ResourceId : IEntityId<long>
{
    public long Value { get; init; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool operator >(ResourceId a, ResourceId b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(ResourceId a, ResourceId b)
    {
        return a.Value < b.Value;
    }
}
