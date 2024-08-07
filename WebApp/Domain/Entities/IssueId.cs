namespace WebApp.Domain.Entities;

public readonly record struct IssueId : IEntityGuid
{
    public Guid Value { get; init; }

    public static readonly IssueId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
