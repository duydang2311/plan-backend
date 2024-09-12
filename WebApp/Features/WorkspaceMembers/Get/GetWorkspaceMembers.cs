using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceMembers.Get;

public sealed record GetWorkspaceMembers : Collective, ICommand<OneOf<PaginatedList<WorkspaceMember>>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public string? Select { get; init; }
}
