using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceInvitations.GetMany;

public sealed record GetWorkspaceInvitations : Collective, ICommand<PaginatedList<WorkspaceInvitation>>
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}
