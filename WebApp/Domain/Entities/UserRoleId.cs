using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct UserRoleId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly UserRoleId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool TryParse(string? input, out UserRoleId output)
    {
        var ok = long.TryParse(input, CultureInfo.InvariantCulture, out var value);
        output = ok ? new UserRoleId { Value = value } : Empty;
        return ok;
    }
}
