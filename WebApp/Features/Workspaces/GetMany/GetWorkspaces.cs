using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.GetMany;

public sealed record GetWorkspaces : Collective, ICommand<PaginatedList<Workspace>>
{
    public WorkspaceId? WorkspaceId { get; init; }
    public UserId? UserId { get; init; }
    public string? Path { get; init; }

    public string? Select { get; init; }
}
