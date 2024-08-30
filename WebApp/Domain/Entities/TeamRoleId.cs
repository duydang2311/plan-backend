using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct TeamRoleId : IEntityId<int>
{
    public int Value { get; init; }

    public static readonly TeamRoleId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool TryParse(string? input, out TeamRoleId output)
    {
        var ok = int.TryParse(input, CultureInfo.InvariantCulture, out var value);
        output = ok ? new TeamRoleId { Value = value } : Empty;
        return ok;
    }
}
