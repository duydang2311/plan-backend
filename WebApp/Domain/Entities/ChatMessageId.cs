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

    public static bool operator >(ChatMessageId a, ChatMessageId b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(ChatMessageId a, ChatMessageId b)
    {
        return a.Value < b.Value;
    }
}
