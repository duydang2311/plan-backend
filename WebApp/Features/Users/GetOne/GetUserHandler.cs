using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.Users.GetOne;

using Result = OneOf<ValidationFailures, NotFoundError, User>;

public sealed class GetUserHandler(AppDbContext db) : ICommandHandler<GetUser, Result>
{
    public async Task<Result> ExecuteAsync(GetUser command, CancellationToken ct)
    {
        var query = db.Users.AsQueryable();

        if (command.UserId.HasValue)
        {
            query = query.Where(a => a.Id == command.UserId.Value);
        }
        else if (!string.IsNullOrEmpty(command.ProfileName))
        {
            query = query.Where(a => a.Profile != null && a.Profile.Name.Equals(command.ProfileName));
        }
        else
        {
            return ValidationFailures
                .Many(2)
                .Add("userId", "require either \"userId\" or \"profileName\"", "partial_required")
                .Add("profileName", "require either \"userId\" or \"profileName\"", "partial_required");
        }

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<User, User>(command.Select));
        }

        var user = await query.FirstOrDefaultAsync(cancellationToken: ct).ConfigureAwait(false);

        return user is null ? new NotFoundError() : user;
    }
}
