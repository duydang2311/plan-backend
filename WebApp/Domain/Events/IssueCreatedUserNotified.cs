namespace WebApp.Domain.Events;

public sealed record IssueCreatedUserNotified : UserNotified
{
    public required long OrderNumber { get; init; }
    public required string Title { get; init; }
    public required string ProjectIdentifier { get; init; }
    public required string WorkspacePath { get; init; }
}
