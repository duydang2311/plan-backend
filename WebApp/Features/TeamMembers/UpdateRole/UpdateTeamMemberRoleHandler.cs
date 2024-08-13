using System.Linq.Dynamic.Core;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.TeamMembers.UpdateRole;

public sealed class UpdateTeamMemberRoleHandler(AppDbContext db)
    : ICommandHandler<UpdateTeamMemberRole, OneOf<ValidationFailures, Success>>
{
    public async Task<OneOf<ValidationFailures, Success>> ExecuteAsync(
        UpdateTeamMemberRole command,
        CancellationToken ct
    )
    {
        if (command.TeamRoleId is null)
        {
            if (string.IsNullOrEmpty(command.RoleName))
            {
                return ValidationFailures
                    .Many(2)
                    .Add("teamRoleId", "Require role name or role id", "required")
                    .Add("roleName", "Require role name or role id", "required");
            }
            var teamRoleId = await db
                .TeamRoles.Where(a => a.Name.Equals(command.RoleName))
                .Select(a => a.Id)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);
            if (teamRoleId == TeamRoleId.Empty)
            {
                return ValidationFailures.Single("roleName", "Invalid role name", "invalid");
            }
            command = command with { TeamRoleId = teamRoleId };
        }

        var count = await db
            .TeamMembers.Where(a => a.TeamId == command.TeamId && a.MemberId == command.MemberId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(x => x.RoleId, command.TeamRoleId), ct)
            .ConfigureAwait(false);

        return count == 0
            ? ValidationFailures
                .Many(2)
                .Add("teamId", "Reference does not exist", "not_found")
                .Add("memberId", "Reference does not exist", "not_found")
            : new Success();
    }
}
