namespace WebApp.Domain.Entities;

public readonly record struct TeamId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly TeamId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
