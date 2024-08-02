using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Workspaces.CreateWorkspace;

using Result = OneOf<IReadOnlyList<ValidationFailure>, Workspace>;

public sealed class CreateWorkspaceHandler(AppDbContext dbContext, IServiceProvider serviceProvider)
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

        await new WorkspaceCreated
        {
            ServiceProvider = serviceProvider,
            UserId = command.UserId,
            Workspace = workspace,
        }
            .PublishAsync(cancellation: ct)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return workspace;
    }
}
