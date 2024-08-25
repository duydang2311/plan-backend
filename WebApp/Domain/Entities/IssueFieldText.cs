namespace WebApp.Domain.Entities;

public sealed record IssueFieldText : IssueField
{
    public string Value { get; init; } = string.Empty;
}
