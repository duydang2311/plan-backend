using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Common.Helpers;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.UserSessions.GetOne.ByToken;

using Result = OneOf<NotFoundError, UserSession>;

public sealed record GetUserSessionByTokenHandler(AppDbContext db) : ICommandHandler<GetUserSessionByToken, Result>
{
    public async Task<Result> ExecuteAsync(GetUserSessionByToken command, CancellationToken ct)
    {
        var query = db.UserSessions.Where(a => a.SessionId == command.Token).AsQueryable();

        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.Select<UserSession, UserSession>(command.Select));
        }

        var userSession = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        if (userSession is null)
        {
            return new NotFoundError();
        }

        return userSession;
    }
}
