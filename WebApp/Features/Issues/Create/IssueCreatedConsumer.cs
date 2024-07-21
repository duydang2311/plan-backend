using Casbin;
using MassTransit;
using WebApp.Domain.Events;

namespace WebApp.Features.Issues.Create;

public sealed class IssueCreatedConsumer(IEnforcer enforcer) : IConsumer<IssueCreated>
{
    public Task Consume(ConsumeContext<IssueCreated> context)
    {
        var sTeamId = context.Message.TeamId.ToString();
        enforcer.AddNamedGroupingPolicy("g2", context.Message.Issue.Id.ToString(), sTeamId, sTeamId);
        return Task.CompletedTask;
    }
}
