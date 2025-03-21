namespace WebApp.Domain.Entities;

public readonly record struct ChatId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly ChatId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
