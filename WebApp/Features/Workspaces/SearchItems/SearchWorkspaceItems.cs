using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.SearchItems;

public sealed record SearchWorkspaceItems : Collective, ICommand<PaginatedList<WorkspaceItem>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string Query { get; init; }
    public string? SelectProject { get; init; }
    public string? SelectIssue { get; init; }
    public float? Threshold { get; init; }
}
