namespace WebApp.Domain.Entities;

public readonly record struct WorkspaceId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly WorkspaceId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
