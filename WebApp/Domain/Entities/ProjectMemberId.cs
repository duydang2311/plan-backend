using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct ProjectMemberId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly ProjectMemberId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
