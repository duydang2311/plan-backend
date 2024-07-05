using Casbin;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.HasWorkspace;

using Result = bool;

public sealed class HasWorkspaceHandler(AppDbContext dbContext, IEnforcer enforcer)
    : ICommandHandler<HasWorkspaceCommand, Result>
{
    public async Task<Result> ExecuteAsync(HasWorkspaceCommand command, CancellationToken ct)
    {
        IQueryable<Workspace> query = dbContext.Workspaces;
        if (command.Id is not null)
        {
            query = query.Where(x => x.Id == command.Id);
        }
        if (command.Path is not null)
        {
            query = query.Where(x => x.Path.Equals(command.Path));
        }
        var workspace = await query.Select(x => new { x.Id }).FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (workspace is null)
        {
            return false;
        }

        return await enforcer
            .EnforceAsync(command.UserId.ToString(), workspace.Id.ToString(), workspace.Id.ToString(), "read")
            .ConfigureAwait(false);
    }
}