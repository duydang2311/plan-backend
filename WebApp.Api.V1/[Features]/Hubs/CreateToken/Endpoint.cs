using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Jwts.Common;

namespace WebApp.Api.V1.Hubs.CreateToken;

public sealed class Endpoint(IJwtService jwtService) : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Post("hubs/token");
        Version(1);
        Description(a => a.ClearDefaultAccepts().Accepts<Request>(true, "*/*"));
    }

    public override Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        return Task.FromResult(
            TypedResults.Ok(
                new Response
                {
                    AccessToken = jwtService.GenerateJwt(
                        audience: "WebApp.Hubs",
                        claims: [new Claim(ClaimTypes.NameIdentifier, req.UserId.ToBase64String())],
                        expires: DateTime.UtcNow.AddMinutes(10)
                    ),
                }
            )
        );
    }
}
