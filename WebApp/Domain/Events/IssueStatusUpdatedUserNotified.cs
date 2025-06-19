namespace WebApp.Domain.Events;

public sealed record IssueStatusUpdatedUserNotified : UserNotified
{
    public required long OrderNumber { get; init; }
    public required string Title { get; init; }
    public required string ProjectIdentifier { get; init; }
    public required string WorkspacePath { get; init; }
    public required byte? OldStatusCategory { get; init; }
    public required string? OldStatusColor { get; init; }
    public required string? OldStatusValue { get; init; }
    public required byte? NewStatusCategory { get; init; }
    public required string? NewStatusColor { get; init; }
    public required string? NewStatusValue { get; init; }
}
