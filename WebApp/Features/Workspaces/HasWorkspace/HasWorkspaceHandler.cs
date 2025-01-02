using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Workspaces.HasWorkspace;

public sealed class HasWorkspaceHandler(AppDbContext dbContext) : ICommandHandler<HasWorkspaceCommand, bool>
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

        return await query.AnyAsync(ct).ConfigureAwait(false);
    }
}
