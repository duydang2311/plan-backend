using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Milestones.GetMany;

public sealed record GetMilestones : Collective, ICommand<PaginatedList<Milestone>>
{
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
}
