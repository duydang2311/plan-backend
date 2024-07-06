using Casbin;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using OneOf;
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

        await Task.WhenAll(dbContext.SaveChangesAsync(ct), CreatePoliciesAsync(command.UserId, workspace.Id))
            .ConfigureAwait(false);
        return workspace;
    }

    private async Task CreatePoliciesAsync(UserId userId, WorkspaceId workspaceId)
    {
        var dom = workspaceId.ToString();
        var sub = dom;
        await enforcer
            .AddPoliciesAsync(
                [
                    ["member", dom, sub, "read"],
                    ["admin", dom, sub, "write"],
                ]
            )
            .ConfigureAwait(false);
        await enforcer
            .AddGroupingPoliciesAsync(
                [
                    ["admin", "member", dom],
                    [userId.ToString(), "admin", dom],
                ]
            )
            .ConfigureAwait(false);
    }
}
