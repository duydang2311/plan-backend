namespace WebApp.Domain.Entities;

public abstract record IssueField
{
    public IssueId IssueId { get; init; }
    public Issue Issue { get; init; } = null!;
    public string Name { get; init; } = string.Empty;
}
