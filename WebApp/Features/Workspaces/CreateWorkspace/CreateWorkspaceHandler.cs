using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.CreateWorkspace;

using Result = OneOf<IReadOnlyList<ValidationFailure>, Workspace>;

public sealed class CreateWorkspaceHandler(AppDbContext dbContext) : ICommandHandler<CreateWorkspaceCommand, Result>
{
    public async Task<Result> ExecuteAsync(CreateWorkspaceCommand command, CancellationToken ct)
    {
        if (await dbContext.Workspaces.AnyAsync(x => x.Path.Equals(command.Path), ct).ConfigureAwait(false))
        {
            return new[] { ValidationHelper.Fail("path", "Path has already been used", "duplicated") };
        }

        var workspace = new Workspace { Name = command.Name, Path = command.Path, };
        dbContext.Add(workspace);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return workspace;
    }
}
