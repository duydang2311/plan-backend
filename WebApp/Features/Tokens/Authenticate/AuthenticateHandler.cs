using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using FastEndpoints;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanoidDotNet;
using OneOf;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Tokens.Authenticate;

using Result = OneOf<IEnumerable<ValidationError>, AuthenticateResult>;

public sealed class AuthenticateHandler(IOptions<JwtOptions> jwtOptions)
    : ICommandHandler<AuthenticateCommand, Result>
{
    public async Task<Result> ExecuteAsync(AuthenticateCommand command, CancellationToken ct)
    {
        if (command is not { Email: "test@gmail.com", Password: "123456" })
        {
            return new[]
            {
                new ValidationError(
                    "email",
                    "Authentication credentials is invalid",
                    "invalid_credentials"
                ),
                new ValidationError(
                    "password",
                    "Authentication credentials is invalid",
                    "invalid_credentials"
                )
            };
        }

        var generateRefreshTokenTask = Nanoid.GenerateAsync();
        var o = jwtOptions.Value;
        var now = DateTime.UtcNow;

        var certificate = X509Certificate2.CreateFromEncryptedPemFile(
            o.CertificateFilePath,
            o.KeyPassword,
            o.KeyFilePath
        );
        var signingCredentials = new SigningCredentials(
            new X509SecurityKey(certificate),
            SecurityAlgorithms.RsaSha256
        );
        var handler = new JwtSecurityTokenHandler();
        var accessTokenMaxAge = TimeSpan.FromMinutes(5);
        var accessToken = handler.CreateJwtSecurityToken(
            issuer: o.ValidIssuers.FirstOrDefault(),
            audience: o.ValidAudiences.FirstOrDefault(),
            subject: new ClaimsIdentity([new Claim(JwtRegisteredClaimNames.Sub, command.Email),]),
            notBefore: now,
            expires: now.Add(accessTokenMaxAge),
            issuedAt: now,
            signingCredentials: signingCredentials
        );

        return new AuthenticateResult(
            handler.WriteToken(accessToken),
            await generateRefreshTokenTask.ConfigureAwait(false),
            (int)accessTokenMaxAge.TotalSeconds,
            (int)TimeSpan.FromDays(1).TotalSeconds
        );
    }
}
