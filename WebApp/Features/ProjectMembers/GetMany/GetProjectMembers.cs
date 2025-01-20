using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectMembers.GetMany;

public sealed record GetProjectMembers : Collective, ICommand<PaginatedList<ProjectMember>>
{
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
}
