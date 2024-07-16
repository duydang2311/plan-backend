using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.HasWorkspace;

public sealed record class HasWorkspaceCommand(UserId UserId, WorkspaceId? Id, string? Path) : ICommand<bool>;
