using FastEndpoints;
using FluentValidation.Results;
using OneOf;
using WebApp.Domain.Entities;

namespace WebApp.Features.Workspaces.CreateWorkspace;

public sealed record class CreateWorkspaceCommand(UserId UserId, string Name, string Path)
    : ICommand<OneOf<IReadOnlyList<ValidationFailure>, Workspace>>;
