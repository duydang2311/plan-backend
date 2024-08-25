namespace WebApp.Domain.Entities;

public sealed record IssueFieldBoolean : IssueField
{
    public bool Value { get; init; }
}
