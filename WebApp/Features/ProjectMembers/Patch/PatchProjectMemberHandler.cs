using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.ProjectMembers.Patch;

public sealed class PatchProjectMemberHandler(AppDbContext dbContext)
    : ICommandHandler<PatchProjectMember, OneOf<ValidationFailures, NotFoundError, Success>>
{
    public async Task<OneOf<ValidationFailures, NotFoundError, Success>> ExecuteAsync(
        PatchProjectMember command,
        CancellationToken ct
    )
    {
        Expression<Func<SetPropertyCalls<ProjectMember>, SetPropertyCalls<ProjectMember>>>? updateEx = default;
        if (command.Patch.TryGetValue(a => a.RoleId, out var roleId) && roleId is not null)
        {
            updateEx = ExpressionHelper.Append(updateEx, a => a.SetProperty(a => a.RoleId, roleId));
        }

        if (updateEx is null)
        {
            return ValidationFailures.Single("patch", "Invalid patch", "invalid");
        }

        var count = await dbContext
            .ProjectMembers.Where(a => a.Id == command.ProjectMemberId)
            .ExecuteUpdateAsync(updateEx, ct)
            .ConfigureAwait(false);

        if (count == 0)
        {
            return new NotFoundError();
        }

        return new Success();
    }
}
