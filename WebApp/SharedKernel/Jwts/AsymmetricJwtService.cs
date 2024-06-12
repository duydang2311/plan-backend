using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.SharedKernel.Jwts.Abstractions;
using WebApp.SharedKernel.Models;

namespace WebApp.SharedKernel.Jwts;

public sealed class AsymmetricJwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;
    private readonly JwtSecurityTokenHandler _handler = new();
    private X509SecurityKey? _x509Key;

    public JwtSecurityToken CreateToken(
        string? issuer = null,
        string? audience = null,
        IEnumerable<Claim>? claims = null,
        DateTime? notBefore = null,
        DateTime? expires = null,
        DateTime? issuedAt = null,
        SigningCredentials? signingCredentials = null
    )
    {
        _x509Key ??= new X509SecurityKey(
            X509Certificate2.CreateFromEncryptedPemFile(
                _options.CertificateFilePath,
                _options.KeyPassword,
                _options.KeyFilePath
            )
        );
        return _handler.CreateJwtSecurityToken(
            issuer: _options.ValidIssuers.FirstOrDefault(),
            audience: _options.ValidAudiences.FirstOrDefault(),
            subject: claims is null ? null : new ClaimsIdentity(claims),
            notBefore: notBefore,
            expires: expires,
            issuedAt: issuedAt,
            signingCredentials: signingCredentials ?? new SigningCredentials(_x509Key, SecurityAlgorithms.RsaSha256)
        );
    }

    public string WriteToken(JwtSecurityToken token)
    {
        return _handler.WriteToken(token);
    }
}
