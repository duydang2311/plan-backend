using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Hubs.CreateToken;

public sealed class Endpoint(IOptions<JwtOptions> jwtOptions) : Endpoint<Request, Ok<Response>>
{
    public override void Configure()
    {
        Post("hubs/token");
        Version(1);
        Description(a => a.ClearDefaultAccepts().Accepts<Request>(true, "*/*"));
    }

    public override Task<Ok<Response>> ExecuteAsync(Request req, CancellationToken ct)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtOptions.Value.PrivateKey);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: "WebApp.Hubs",
            claims: [new Claim(ClaimTypes.NameIdentifier, req.UserId.ToBase64String())],
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        );

        return Task.FromResult(
            TypedResults.Ok(new Response { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) })
        );
    }
}
