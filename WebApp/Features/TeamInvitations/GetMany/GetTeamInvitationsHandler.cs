using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamInvitations.GetMany;

public sealed record GetTeamInvitationHandlers(AppDbContext db)
    : ICommandHandler<GetTeamInvitations, PaginatedList<TeamInvitation>>
{
    public async Task<PaginatedList<TeamInvitation>> ExecuteAsync(GetTeamInvitations command, CancellationToken ct)
    {
        var query = db.TeamInvitations.AsQueryable();

        if (command.TeamId is not null)
        {
            query = query.Where(x => x.TeamId == command.TeamId);
        }
        if (command.MemberId is not null)
        {
            query = query.Where(x => x.MemberId == command.MemberId);
        }
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.LambdaNew<TeamInvitation>(command.Select));
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);
        var items = await query.Skip(command.Offset).Take(command.Size).ToArrayAsync(ct).ConfigureAwait(false);
        return new() { Items = items, TotalCount = totalCount };
    }
}
