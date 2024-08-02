using Casbin;
using FastEndpoints;
using WebApp.Common.Constants;
using WebApp.Domain.Events;

namespace WebApp.Features.Workspaces.CreateWorkspace;

public sealed class WorkspaceCreatedHandler : IEventHandler<WorkspaceCreated>
{
    public Task HandleAsync(WorkspaceCreated eventModel, CancellationToken ct)
    {
        var enforcer = eventModel.ServiceProvider.GetRequiredService<IEnforcer>();
        var sUserId = eventModel.UserId.ToString();
        var sWorkspaceId = eventModel.Workspace.Id.ToString();
        enforcer.AddPolicy("admin", sWorkspaceId, sWorkspaceId, Permit.Read);
        enforcer.AddPolicy("admin", sWorkspaceId, sWorkspaceId, Permit.WriteTeam);
        enforcer.AddGroupingPolicy(sUserId, "admin", sWorkspaceId);
        return Task.CompletedTask;
    }
}
