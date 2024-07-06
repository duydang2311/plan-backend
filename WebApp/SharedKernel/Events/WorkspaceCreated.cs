using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Events;

public sealed record WorkspaceCreated
{
    public required Workspace Workspace { get; init; }
    public UserId UserId { get; init; }
}
