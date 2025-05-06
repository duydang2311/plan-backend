using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct RoleId : IEntityId<int>
{
    public int Value { get; init; }

    public static readonly RoleId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static RoleId From(int value)
    {
        return new RoleId { Value = value };
    }

    public static bool operator >(RoleId a, RoleId b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(RoleId a, RoleId b)
    {
        return a.Value < b.Value;
    }
}
