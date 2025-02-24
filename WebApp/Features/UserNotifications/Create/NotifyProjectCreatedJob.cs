using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.UserNotifications.Create;

public sealed record NotifyProjectCreatedJob : ICommand
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required ProjectId ProjectId { get; init; }
}
