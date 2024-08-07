using Casbin;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Workspaces.HasWorkspace;

public sealed class HasWorkspaceHandler(AppDbContext dbContext, IEnforcer enforcer)
    : ICommandHandler<HasWorkspaceCommand, bool>
{
    public async Task<bool> ExecuteAsync(HasWorkspaceCommand command, CancellationToken ct)
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
            .EnforceAsync(command.UserId.ToString(), workspace.Id.ToString(), workspace.Id.ToString(), Permit.Read)
            .ConfigureAwait(false);
    }
}
