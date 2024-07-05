using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Events;

public sealed record class WorkspaceCreatedEvent(Workspace Workspace, UserId UserId) : IEvent;
