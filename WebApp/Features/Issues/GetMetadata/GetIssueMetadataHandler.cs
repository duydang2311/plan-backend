using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Issues.GetMetadata;

public sealed class GetIssueMetadataHandler(AppDbContext dbContext)
    : ICommandHandler<GetIssueMetadata, GetIssueMetadataResult>
{
    public async Task<GetIssueMetadataResult> ExecuteAsync(GetIssueMetadata command, CancellationToken ct)
    {
        var query = dbContext.Issues.AsQueryable();

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        if (!command.TeamId.HasValue && !command.ProjectId.HasValue)
        {
            query = query.Where(a =>
                a.Project.Members.Any(b =>
                    b.UserId == command.UserId && b.Role.Permissions.Any(c => c.Permission.Equals(Permit.ReadIssue))
                )
                || a.Teams.Any(b =>
                    b.TeamMembers.Any(b =>
                        b.MemberId == command.UserId
                        && b.Role.Permissions.Any(c => c.Permission.Equals(Permit.ReadIssue))
                    )
                )
            );
        }
        else
        {
            if (command.ProjectId.HasValue)
            {
                query = query.Where(a => a.ProjectId == command.ProjectId.Value);
            }
            if (command.TeamId.HasValue)
            {
                query = query.Where(a => a.Teams.Any(b => b.Id == command.TeamId.Value));
            }
        }

        if (command.StatusId.HasValue)
        {
            query = query.Where(a => a.StatusId == command.StatusId);
        }
        else if (command.NullStatusId == true)
        {
            query = query.Where(a => a.StatusId == null);
        }

        return new GetIssueMetadataResult
        {
            TotalCount = totalCount,
            Count = await query.CountAsync(ct).ConfigureAwait(false)
        };
    }
}
