namespace WebApp.Features.ProjectMembers.GetMetadata;

public sealed record GetProjectMemberMetadataResult
{
    public int Count { get; init; }
    public int TotalCount { get; init; }
}
