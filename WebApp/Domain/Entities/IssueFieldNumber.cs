namespace WebApp.Domain.Entities;

public sealed record IssueFieldNumber : IssueField
{
    public int Value { get; init; }
}
