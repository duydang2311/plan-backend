namespace WebApp.Domain.Entities;

public readonly record struct IssueCommentId : IEntityId
{
    public Guid Value { get; init; }

    public static readonly IssueCommentId Empty = new() { Value = Guid.Empty };

    public override string ToString()
    {
        return Value.ToString();
    }
}
