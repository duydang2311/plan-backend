using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Workspaces.HasWorkspace;

public sealed record class HasWorkspaceCommand(UserId UserId, WorkspaceId? Id, string? Path) : ICommand<bool>;
