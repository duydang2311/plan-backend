namespace WebApp.Domain.Entities;

public readonly record struct UserId : IEntityId
{
    public Guid Value { get; init; }

    public static readonly UserId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
