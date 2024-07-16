namespace WebApp.Domain.Entities;

public readonly record struct WorkspaceId(Guid Value)
{
    public static readonly WorkspaceId Empty = new(Guid.Empty);

    public override string ToString()
    {
        return Value.ToString();
    }
}
