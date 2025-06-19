using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using WebApp.Domain.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Workspaces.GetAnalytics;

public sealed record GetWorkspaceAnalyticsHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceAnalytics, GetWorkspaceAnalyticsResult>
{
    public async Task<GetWorkspaceAnalyticsResult> ExecuteAsync(GetWorkspaceAnalytics command, CancellationToken ct)
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        var nextWeek = now + Duration.FromDays(7);
        return await db
                .Workspaces.Where(a => a.Id == command.WorkspaceId)
                .Select(a => new GetWorkspaceAnalyticsResult
                {
                    TotalProjects = a.Projects.Count,
                    ActiveIssues = a
                        .Projects.SelectMany(p => p.Issues)
                        .Count(i => i.Status != null && i.Status.Category == StatusCategory.Ongoing),
                    TotalMembers = a.Members.Count,
                    IssuesDueThisWeek = a
                        .Projects.SelectMany(p => p.Issues)
                        .Count(i => i.EndTime.HasValue && i.EndTime.Value >= now && i.EndTime.Value < nextWeek),
                })
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false)
            ?? new GetWorkspaceAnalyticsResult
            {
                TotalProjects = 0,
                ActiveIssues = 0,
                TotalMembers = 0,
                IssuesDueThisWeek = 0,
            };
    }
}
