using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.GetMany;

public sealed record GetWorkspaceResources : KeysetPagination<ResourceId?>, ICommand<PaginatedList<WorkspaceResource>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public string? Select { get; init; }
}
