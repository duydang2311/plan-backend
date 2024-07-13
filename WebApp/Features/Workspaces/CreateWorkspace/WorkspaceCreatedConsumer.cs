using Casbin;
using MassTransit;
using WebApp.Domain.Events;
using WebApp.SharedKernel.Constants;

namespace WebApp.Features.Workspaces.CreateWorkspace;

public sealed class WorkspaceCreatedConsumer(IEnforcer enforcer) : IConsumer<WorkspaceCreated>
{
    public Task Consume(ConsumeContext<WorkspaceCreated> context)
    {
        var sUserId = context.Message.UserId.ToString();
        var sWorkspaceId = context.Message.Workspace.Id.ToString();
        enforcer.AddPolicy("admin", sWorkspaceId, sWorkspaceId, Permit.Read);
        enforcer.AddPolicy("admin", sWorkspaceId, sWorkspaceId, Permit.WriteTeam);
        enforcer.AddGroupingPolicy(sUserId, "admin", sWorkspaceId);
        return Task.CompletedTask;
    }
}
