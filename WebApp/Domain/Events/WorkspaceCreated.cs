using WebApp.SharedKernel.Models;

namespace WebApp.Domain.Events;

public sealed record WorkspaceCreated
{
    public required Workspace Workspace { get; init; }
    public required UserId UserId { get; init; }
}
