using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly struct IssueLinkId : IEntityId<long>
{
    public long Value { get; init; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
