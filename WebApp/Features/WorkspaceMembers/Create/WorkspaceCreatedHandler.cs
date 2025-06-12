using FastEndpoints;
using WebApp.Domain.Constants;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceMembers.Create;

public sealed class WorkspaceCreatedHandler : IEventHandler<WorkspaceCreated>
{
    public Task HandleAsync(WorkspaceCreated eventModel, CancellationToken ct)
    {
        var db = eventModel.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Add(
            new WorkspaceMember
            {
                RoleId = WorkspaceRoleDefaults.Owner.Id,
                UserId = eventModel.UserId,
                WorkspaceId = eventModel.Workspace.Id,
            }
        );
        return Task.CompletedTask;
    }
}
