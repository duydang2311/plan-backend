using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Domain.Entities;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Workspaces.Get;

public sealed class GetWorkspaceHandler(AppDbContext dbContext) : ICommandHandler<GetWorkspace, OneOf<None, Workspace>>
{
    public async Task<OneOf<None, Workspace>> ExecuteAsync(GetWorkspace command, CancellationToken ct)
    {
        var query = dbContext.Workspaces.AsQueryable();
        if (command.WorkspaceId is not null)
        {
            query = query.Where(x => x.Id == command.WorkspaceId.Value);
        }
        if (command.Path is not null)
        {
            query = query.Where(x => x.Path == command.Path);
        }
        query = string.IsNullOrEmpty(command.Select)
            ? query.Select(x => new Workspace { Id = x.Id, Name = x.Name })
            : query.Select<Workspace>(command.Select);

        var workspace = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return workspace is null ? new None() : workspace;
    }
}
