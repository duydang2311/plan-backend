using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Projects.Search;

public sealed record SearchProjects : Collective, ICommand<PaginatedList<Project>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string Query { get; init; }
    public string? Select { get; init; }
    public float? Threshold { get; init; }
}
