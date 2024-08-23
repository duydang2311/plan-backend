using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamInvitations.Decline;

using Result = OneOf<ValidationFailures, Success>;

public sealed class DeclineTeamInvitationHandler(AppDbContext db) : ICommandHandler<DeclineTeamInvitation, Result>
{
    public async Task<Result> ExecuteAsync(DeclineTeamInvitation command, CancellationToken ct)
    {
        var query = db.TeamInvitations.AsQueryable();
        if (command.TeamInvitationId is not null)
        {
            query = query.Where(a => a.Id == command.TeamInvitationId);
        }
        else if (command.TeamId is not null && command.MemberId is not null)
        {
            query = query.Where(a => a.TeamId == command.TeamId && a.MemberId == command.MemberId);
        }
        else
        {
            return ValidationFailures.Single(
                "identifier",
                "Properties 'teamInvitationId' or 'teamId', 'memberId' are required",
                "required"
            );
        }

        var count = await query.ExecuteDeleteAsync(ct).ConfigureAwait(false);
        if (count == 0)
        {
            return ValidationFailures.Single("identifier", "Reference does not exist", "not_found");
        }

        return new Success();
    }
}
