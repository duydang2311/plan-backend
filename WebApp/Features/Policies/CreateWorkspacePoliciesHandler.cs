using Casbin;
using FastEndpoints;
using WebApp.SharedKernel.Events;

namespace WebApp.Features.Policies;

public sealed class CreateWorkspacePoliciesHandler(IEnforcer enforcer) : IEventHandler<WorkspaceCreatedEvent>
{
    public async Task HandleAsync(WorkspaceCreatedEvent eventModel, CancellationToken ct)
    {
        var domain = eventModel.Workspace.Id.Value.ToString();
        var obj = eventModel.Workspace.Id.Value.ToString();
        await enforcer.AddPolicyAsync("member", domain, obj, "read").ConfigureAwait(false);
        await enforcer.AddPolicyAsync("admin", domain, obj, "write").ConfigureAwait(false);
        await enforcer.AddGroupingPolicyAsync("admin", "member", domain).ConfigureAwait(false);
        await enforcer
            .AddGroupingPolicyAsync(eventModel.UserId.Value.ToString(), "admin", domain)
            .ConfigureAwait(false);
    }
}
