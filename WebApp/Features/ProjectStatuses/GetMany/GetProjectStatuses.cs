using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectStatuses.GetMany;

public sealed record GetProjectStatuses : Collective, ICommand<OneOf<PaginatedList<ProjectStatus>>>
{
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
}
