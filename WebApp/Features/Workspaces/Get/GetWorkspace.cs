using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.Get;

public sealed record GetWorkspace : ICommand<OneOf<None, Workspace>>
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Path { get; init; }

    public string? Select { get; init; }
}
