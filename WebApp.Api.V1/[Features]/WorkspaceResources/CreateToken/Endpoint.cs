using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Jwts.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.WorkspaceResources.CreateToken;

using Results = Results<NotFound, Ok<Response>>;

public sealed class Endpoint(AppDbContext db, IJwtService jwtService) : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Post("workspace-resources/token");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var resourceFile = await db
            .ResourceFiles.Where(a => a.Id == req.ResourceFileId!.Value)
            .Select(a => new { a.Key })
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);

        if (resourceFile is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(
            new Response
            {
                AccessToken = jwtService.GenerateJwt(
                    audience: "coop-cf-worker",
                    claims:
                    [
                        new Claim(JwtRegisteredClaimNames.Jti, IdHelper.NewRandomId()),
                        new Claim(ClaimTypes.NameIdentifier, req.UserId.ToBase64String()),
                        new Claim("workspaceId", req.WorkspaceId!.Value.ToBase64String()),
                        new Claim("object_key", resourceFile.Key),
                    ],
                    expires: DateTime.UtcNow.AddMinutes(1)
                ),
            }
        );
    }
}
