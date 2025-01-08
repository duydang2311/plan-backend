namespace WebApp.Features.ProjectTeams.GetMetadata;

public sealed record GetProjectTeamMetadataResult
{
    public int Count { get; init; }
    public int TotalCount { get; init; }
}
