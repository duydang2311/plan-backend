using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectTeams.GetMetadata;

public sealed record GetProjectTeamMetadataHandler(AppDbContext db)
    : ICommandHandler<GetProjectTeamMetadata, GetProjectTeamMetadataResult>
{
    public async Task<GetProjectTeamMetadataResult> ExecuteAsync(GetProjectTeamMetadata command, CancellationToken ct)
    {
        var query = db.ProjectTeams.AsQueryable();

        var totalCount = command.IncludeTotalCount ? await query.CountAsync(ct).ConfigureAwait(false) : 0;

        if (command.ProjectId.HasValue)
        {
            query = query.Where(a => a.ProjectId == command.ProjectId.Value);
        }
        return new GetProjectTeamMetadataResult
        {
            Count = await query.CountAsync(ct).ConfigureAwait(false),
            TotalCount = totalCount
        };
    }
}
