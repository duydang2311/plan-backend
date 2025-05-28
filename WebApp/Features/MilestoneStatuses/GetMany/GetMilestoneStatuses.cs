using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.MilestoneStatuses.GetMany;

public sealed record GetMilestoneStatuses : Collective, ICommand<PaginatedList<MilestoneStatus>>
{
    public required ProjectId ProjectId { get; init; }
    public string? Select { get; init; }
}
