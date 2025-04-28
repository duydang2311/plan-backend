using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.GetOne;

public sealed record GetWorkspaceResourceHandler(AppDbContext db)
    : ICommandHandler<GetWorkspaceResource, OneOf<NotFoundError, WorkspaceResource>>
{
    public async Task<OneOf<NotFoundError, WorkspaceResource>> ExecuteAsync(
        GetWorkspaceResource command,
        CancellationToken ct
    )
    {
        var query = db.WorkspaceResources.Where(a => a.ResourceId == command.Id);

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<WorkspaceResource, WorkspaceResource>(command.Select));
        }

        var workspaceResource = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (workspaceResource is null)
        {
            return new NotFoundError();
        }

        return workspaceResource;
    }
}
