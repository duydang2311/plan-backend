using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.GetAnalytics;

public sealed record GetWorkspaceAnalytics : ICommand<GetWorkspaceAnalyticsResult>
{
    public required WorkspaceId WorkspaceId { get; init; }
}
