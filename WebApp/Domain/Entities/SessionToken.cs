namespace WebApp.Domain.Entities;

public readonly record struct SessionToken : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly SessionToken Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
