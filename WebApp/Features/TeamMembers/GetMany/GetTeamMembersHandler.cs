using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamMembers.GetMany;

using Result = PaginatedList<TeamMember>;

public sealed class GetTeamMembersHandler(AppDbContext dbContext) : ICommandHandler<GetTeamMembers, Result>
{
    public async Task<Result> ExecuteAsync(GetTeamMembers command, CancellationToken ct)
    {
        var query = dbContext.TeamMembers.Where(a => a.TeamId == command.TeamId);
        if (command.Select is not null)
        {
            query = query.Select(ExpressionHelper.LambdaNew<TeamMember>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        query = command
            .Order.Where(static a =>
                a.Name.EqualsEither(["CreatedTime", "UpdatedTime"], StringComparison.OrdinalIgnoreCase)
            )
            .SortOrDefault(query, a => a.OrderByDescending(x => x.CreatedTime));
        var teamMembers = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);

        return new() { Items = teamMembers, TotalCount = totalCount, };
    }
}
