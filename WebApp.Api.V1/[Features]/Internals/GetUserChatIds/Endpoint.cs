using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.Internals.GetUserChatIds;

public sealed class Endpoint(AppDbContext db) : EndpointWithoutRequest<Ok<List<ChatId>>>
{
    public override void Configure()
    {
        Get("internals/get-user-chat-ids");
        Version(1);
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task<Ok<List<ChatId>>> ExecuteAsync(CancellationToken ct)
    {
        var userId = Query<UserId>("userId", isRequired: true);
        var ids = await db
            .ChatMembers.Where(a => a.MemberId == userId)
            .Select(a => a.ChatId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
        return TypedResults.Ok(ids ?? []);
    }
}
