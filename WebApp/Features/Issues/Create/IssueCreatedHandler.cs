using Casbin;
using FastEndpoints;
using WebApp.Domain.Events;

namespace WebApp.Features.Issues.Create;

public sealed class IssueCreatedConsumer : IEventHandler<IssueCreated>
{
    public Task HandleAsync(IssueCreated eventModel, CancellationToken ct)
    {
        var enforcer = eventModel.ServiceProvider.GetRequiredService<IEnforcer>();
        var sTeamId = eventModel.TeamId.ToString();
        enforcer.AddNamedGroupingPolicy("g2", eventModel.Issue.Id.ToString(), sTeamId, sTeamId);
        return Task.CompletedTask;
    }
}
