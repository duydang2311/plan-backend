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

    public static bool TryParse(string? input, out RoleId output)
    {
        var ok = int.TryParse(input, CultureInfo.InvariantCulture, out var value);
        output = ok ? new RoleId { Value = value } : Empty;
        return ok;
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
