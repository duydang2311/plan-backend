using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct ChecklistItemId : IEntityId<long>
{
    public long Value { get; init; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool operator >(ChecklistItemId a, ChecklistItemId b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(ChecklistItemId a, ChecklistItemId b)
    {
        return a.Value < b.Value;
    }
}
