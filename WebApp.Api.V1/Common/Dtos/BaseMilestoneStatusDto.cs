using WebApp.Domain.Constants;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

public sealed record BaseMilestoneStatusDto
{
    public MilestoneStatusId? Id { get; init; }
    public MilestoneStatusCategory? Category { get; init; }
    public string? Rank { get; init; }
    public string? Value { get; init; }
    public string? Color { get; init; }
    public string? Icon { get; init; }
    public string? Description { get; init; }
    public ProjectId? ProjectId { get; init; }
    public BaseProjectDto? Project { get; init; }
    public bool? IsDefault { get; init; }
}
