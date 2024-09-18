using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceStatuses.GetMany;

public sealed record GetWorkspaceStatuses : Collective, ICommand<OneOf<PaginatedList<WorkspaceStatus>>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public string? Select { get; init; }
}
