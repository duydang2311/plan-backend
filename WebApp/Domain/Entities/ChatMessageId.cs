using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct ChatMessageId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly ChatMessageId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
