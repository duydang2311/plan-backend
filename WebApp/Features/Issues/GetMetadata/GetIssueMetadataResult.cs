namespace WebApp.Features.Issues.GetMetadata;

public sealed record GetIssueMetadataResult
{
    public int Count { get; init; }
    public int TotalCount { get; init; }
}
