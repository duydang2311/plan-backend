namespace WebApp.Domain.Entities;

public readonly record struct ProjectId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly ProjectId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
