namespace WebApp.Domain.Events;

public sealed record ProjectCreatedUserNotified : UserNotified
{
    public required string Identifier { get; init; }
    public required string Name { get; init; }
    public required string WorkspacePath { get; init; }
}
