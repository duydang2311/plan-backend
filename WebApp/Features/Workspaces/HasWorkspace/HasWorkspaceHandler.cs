using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.HasWorkspace;

using Result = bool;

public sealed class HasWorkspaceHandler(AppDbContext dbContext) : ICommandHandler<HasWorkspaceCommand, Result>
{
    public Task<Result> ExecuteAsync(HasWorkspaceCommand command, CancellationToken ct)
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
        return query.AnyAsync(ct);
    }
}
