using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Projects.GetMany;

public sealed record GetProjects : Collective, ICommand<OneOf<PaginatedList<Project>>>
{
    public WorkspaceId? WorkspaceId { get; init; }
    public string? Select { get; init; }
}
