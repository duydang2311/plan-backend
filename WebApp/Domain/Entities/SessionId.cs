namespace WebApp.Domain.Entities;

public readonly record struct SessionId : IEntityId<string>
{
    public string Value { get; init; }

    public static readonly SessionId Empty = new() { Value = string.Empty };

    public override string ToString()
    {
        return Value;
    }
}
