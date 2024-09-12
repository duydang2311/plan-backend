using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

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

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<Workspace, Workspace>(command.Select));
        }

        var workspace = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return workspace is null ? new None() : workspace;
    }
}
