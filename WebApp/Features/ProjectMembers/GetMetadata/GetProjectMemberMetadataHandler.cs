using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.GetMetadata;

public sealed record GetProjectMemberMetadataHandler(AppDbContext db)
    : ICommandHandler<GetProjectMemberMetadata, GetProjectMemberMetadataResult>
{
    public async Task<GetProjectMemberMetadataResult> ExecuteAsync(
        GetProjectMemberMetadata command,
        CancellationToken ct
    )
    {
        var query = db.ProjectMembers.AsQueryable();

        var totalCount = command.IncludeTotalCount ? await query.CountAsync(ct).ConfigureAwait(false) : 0;

        if (command.ProjectId.HasValue)
        {
            query = query.Where(a => a.ProjectId == command.ProjectId.Value);
        }

        return new GetProjectMemberMetadataResult
        {
            Count = await query.CountAsync(ct).ConfigureAwait(false),
            TotalCount = totalCount
        };
    }
}
