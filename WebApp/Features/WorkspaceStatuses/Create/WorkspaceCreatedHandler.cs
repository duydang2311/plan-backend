using FastEndpoints;
using WebApp.Domain.Constants;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceStatuses.Create;

public sealed class WorkspaceCreatedHandler : IEventHandler<WorkspaceCreated>
{
    public Task HandleAsync(WorkspaceCreated eventModel, CancellationToken ct)
    {
        var db = eventModel.ServiceProvider.GetRequiredService<AppDbContext>();
        db.AddRange(StatusDefaults.WorkspaceStatuses.Select(a => a with { WorkspaceId = eventModel.Workspace.Id }));
        return Task.CompletedTask;
    }
}
