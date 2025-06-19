using Riok.Mapperly.Abstractions;
using WebApp.Features.Workspaces.GetAnalytics;

namespace WebApp.Api.V1.Workspaces.GetAnalytics;

public sealed record Response
{
    public required int TotalProjects { get; init; }
    public required int ActiveIssues { get; init; }
    public required int TotalMembers { get; init; }
    public required int IssuesDueThisWeek { get; init; }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this GetWorkspaceAnalyticsResult result);
}
