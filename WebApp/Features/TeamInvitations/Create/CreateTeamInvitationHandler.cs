using EntityFramework.Exceptions.Common;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamInvitations.Create;

using Result = OneOf<ValidationFailures, TeamInvitation>;

public sealed class CreateTeamInvitationHandler(AppDbContext db) : ICommandHandler<CreateTeamInvitation, Result>
{
    public async Task<Result> ExecuteAsync(CreateTeamInvitation command, CancellationToken ct)
    {
        if (
            await db
                .TeamMembers.AnyAsync(a => a.TeamId == command.TeamId && a.MemberId == command.MemberId, ct)
                .ConfigureAwait(false)
        )
        {
            return ValidationFailures
                .Many(2)
                .Add("teamId", "The member is already in the team", "in_team")
                .Add("memberId", "The member is already in the team", "in_team");
        }

        var teamInvitation = new TeamInvitation { TeamId = command.TeamId, MemberId = command.MemberId, };
        db.Add(teamInvitation);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException exception)
        {
            var failures = ValidationFailures.Many(exception.ConstraintProperties.Count);
            foreach (var property in exception.ConstraintProperties)
            {
                failures.Add(property, $"{property} does not exist", "not_found");
            }
            return failures;
        }
        catch (UniqueConstraintException exception)
        {
            var failures = ValidationFailures.Many(exception.ConstraintProperties.Count);
            foreach (var property in exception.ConstraintProperties)
            {
                failures.Add(property, "Invitation already exists", "duplicated");
            }
            return failures;
        }
        return teamInvitation;
    }
}
