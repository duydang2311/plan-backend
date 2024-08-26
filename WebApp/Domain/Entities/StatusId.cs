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
}
