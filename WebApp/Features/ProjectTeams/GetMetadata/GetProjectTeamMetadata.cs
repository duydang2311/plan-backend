using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.ProjectTeams.GetMetadata;

public sealed record GetProjectTeamMetadata : ICommand<GetProjectTeamMetadataResult>
{
    public ProjectId? ProjectId { get; init; }
    public bool IncludeTotalCount { get; init; }
}
