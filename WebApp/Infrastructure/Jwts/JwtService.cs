using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.Infrastructure.Jwts.Common;

namespace WebApp.Infrastructure.Jwts;

public sealed class JwtService : IJwtService
{
    readonly SigningCredentials signingCredentials;
    readonly IOptions<JwtOptions> jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        this.jwtOptions = jwtOptions;

        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtOptions.Value.PrivateKey);
        signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateJwt(string audience, IEnumerable<Claim> claims, DateTime expires)
    {
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
