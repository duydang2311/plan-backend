using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.SharedKernel.Events;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.CreateWorkspace;

using Result = OneOf<IReadOnlyList<ValidationFailure>, Workspace>;

public sealed class CreateWorkspaceHandler(AppDbContext dbContext) : CommandHandler<CreateWorkspaceCommand, Result>
{
    public override async Task<Result> ExecuteAsync(CreateWorkspaceCommand command, CancellationToken ct)
    {
        if (await dbContext.Workspaces.AnyAsync(x => x.Path.Equals(command.Path), ct).ConfigureAwait(false))
        {
            return new[] { ValidationHelper.Fail("path", "Path has already been used", "duplicated") };
        }

        var workspace = new Workspace { Name = command.Name, Path = command.Path, };
        dbContext.Add(workspace);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        await new WorkspaceCreatedEvent(workspace, command.UserId)
            .PublishAsync(Mode.WaitForNone, ct)
            .ConfigureAwait(false);

        return workspace;
    }
}
