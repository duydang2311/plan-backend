using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.SharedKernel.Authorization.Abstractions;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.CreateWorkspace;

using Result = OneOf<IReadOnlyList<ValidationFailure>, Workspace>;

public sealed class CreateWorkspaceHandler(AppDbContext dbContext, IEnforcer enforcer)
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
            Id = new WorkspaceId(Guid.NewGuid()),
            Name = command.Name,
            Path = command.Path,
        };
        dbContext.Add(workspace);
        AddPolicies(command.UserId, workspace.Id);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return workspace;
    }

    private void AddPolicies(UserId userId, WorkspaceId workspaceId)
    {
        var sub = userId.ToString();
        var obj = workspaceId.ToString();
        enforcer.Add(sub, obj, Permit.Read);
        enforcer.Add(sub, obj, Permit.CreateTeam);
    }
}
