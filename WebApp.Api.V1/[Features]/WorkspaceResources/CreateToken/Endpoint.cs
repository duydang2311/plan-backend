using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.JsonWebTokens;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Jwts.Common;

namespace WebApp.Api.V1.WorkspaceResources.CreateToken;

public sealed class Endpoint(IJwtService jwtService) : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Post("workspace-resources/token");
        Version(1);
    }

    public override Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        return Task.FromResult(
            TypedResults.Ok(
                new Response
                {
                    AccessToken = jwtService.GenerateJwt(
                        audience: "coop-cf-worker",
                        claims:
                        [
                            new Claim(JwtRegisteredClaimNames.Jti, IdHelper.NewRandomId()),
                            new Claim(ClaimTypes.NameIdentifier, req.UserId.ToBase64String()),
                            new Claim("workspaceId", req.WorkspaceId!.Value.ToBase64String()),
                        ],
                        expires: DateTime.UtcNow.AddMinutes(1)
                    ),
                }
            )
        );
    }
}
