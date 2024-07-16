using FastEndpoints;
using FluentValidation.Results;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.CreateWorkspace;

using Result = OneOf<IReadOnlyList<ValidationFailure>, Workspace>;

public sealed class CreateWorkspaceHandler(AppDbContext dbContext, IScopedMediator mediator)
    : CommandHandler<CreateWorkspaceCommand, Result>
{
    public override async Task<Result> ExecuteAsync(CreateWorkspaceCommand command, CancellationToken ct)
    {
        if (await dbContext.Workspaces.AnyAsync(x => x.Path.Equals(command.Path), ct).ConfigureAwait(false))
        {
            return new[] { ValidationHelper.Fail("path", "Path has already been used", "duplicated") };
        }

        var workspace = new Workspace
        {
            Id = IdHelper.NewWorkspaceId(),
            Name = command.Name,
            Path = command.Path,
        };
        dbContext.Add(workspace);

        await mediator
            .Publish(new WorkspaceCreated { UserId = command.UserId, Workspace = workspace, }, ct)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return workspace;
    }
}
