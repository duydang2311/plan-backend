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
}
