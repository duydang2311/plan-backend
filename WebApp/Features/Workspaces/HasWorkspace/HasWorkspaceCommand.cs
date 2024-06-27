using FastEndpoints;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Workspaces.HasWorkspace;

public sealed record class HasWorkspaceCommand(WorkspaceId? Id, string? Path) : ICommand<bool>;
