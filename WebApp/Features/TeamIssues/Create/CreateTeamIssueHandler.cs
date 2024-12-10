using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamIssues.Create;

using Result = OneOf<ValidationFailures, Success>;

public sealed class CreateTeamIssueHandler(AppDbContext db) : ICommandHandler<CreateTeamIssue, Result>
{
    public async Task<Result> ExecuteAsync(CreateTeamIssue command, CancellationToken ct)
    {
        var teamIssue = new TeamIssue { TeamId = command.TeamId, IssueId = command.IssueId };
        db.Add(teamIssue);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException e)
        {
            var failures = ValidationFailures.Many(2);
            foreach (var p in e.ConstraintProperties)
            {
                if (p.Equals(nameof(TeamIssue.TeamId)))
                {
                    failures.Add("team_id", "Team does not exist", "invalid_reference");
                }
                else if (p.Equals(nameof(TeamIssue.IssueId)))
                {
                    failures.Add("issue_id", "Issue does not exist", "invalid_reference");
                }
                else
                {
                    failures.Add(p, $"{p} does not exist", "invalid_reference");
                }
            }
            return failures;
        }
        catch (UniqueConstraintException e)
        {
            var failures = ValidationFailures.Many(2);
            foreach (var p in e.ConstraintProperties)
            {
                if (p.Equals(nameof(TeamIssue.TeamId)))
                {
                    failures.Add("team_id", "Issue already belongs to this team", "unique");
                }
                if (p.Equals(nameof(TeamIssue.IssueId)))
                {
                    failures.Add("issue_id", "Issue already belongs to this team", "unique");
                }
                else
                {
                    failures.Add(p, "Issue already belongs to this team", "unique");
                }
            }
            return failures;
        }
        return new Success();
    }
}
