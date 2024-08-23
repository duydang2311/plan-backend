using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Events;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamInvitations.Accept;

using Result = OneOf<ValidationFailures, Success>;

public sealed class AcceptTeamInvitationHandler(AppDbContext db, IServiceProvider serviceProvider)
    : ICommandHandler<AcceptTeamInvitation, Result>
{
    public async Task<Result> ExecuteAsync(AcceptTeamInvitation command, CancellationToken ct)
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

        var invitation = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (invitation is null)
        {
            return ValidationFailures.Single("identifier", "Reference does not exist", "not_found");
        }

        await query.ExecuteDeleteAsync(ct).ConfigureAwait(false);
        await new TeamInvitationAccepted { TeamInvitation = invitation, ServiceProvider = serviceProvider }
            .PublishAsync(Mode.WaitForAll, ct)
            .ConfigureAwait(false);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        return new Success();
    }
}
