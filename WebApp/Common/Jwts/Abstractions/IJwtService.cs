using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Common.Jwts.Abstractions;

public interface IJwtService
{
    JwtSecurityToken CreateToken(
        string? issuer = null,
        string? audience = null,
        IEnumerable<Claim>? claims = null,
        DateTime? notBefore = null,
        DateTime? expires = null,
        DateTime? issuedAt = null,
        SigningCredentials? signingCredentials = null
    );
    string WriteToken(JwtSecurityToken token);
}
