using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct StatusId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly StatusId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool TryParse(string? input, out StatusId output)
    {
        var ok = long.TryParse(input, CultureInfo.InvariantCulture, out var value);
        output = ok ? new StatusId { Value = value } : Empty;
        return ok;
    }
}
