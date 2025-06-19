namespace WebApp.Features.Workspaces.GetAnalytics;

public sealed record GetWorkspaceAnalyticsResult
{
    public required int TotalProjects { get; init; }
    public required int ActiveIssues { get; init; }
    public required int TotalMembers { get; init; }
    public required int IssuesDueThisWeek { get; init; }
}
