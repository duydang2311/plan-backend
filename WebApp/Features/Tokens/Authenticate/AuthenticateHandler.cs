using FastEndpoints;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NanoidDotNet;
using OneOf;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WebApp.SharedKernel.Models;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace WebApp.Features.Tokens.Authenticate;

public sealed class AuthenticateHandler(IOptions<JwtOptions> jwtOptions) : ICommandHandler<AuthenticateCommand, OneOf<ProblemDetails, AuthenticateResult>>
{
    public async Task<OneOf<ProblemDetails, AuthenticateResult>> ExecuteAsync(AuthenticateCommand command, CancellationToken ct)
    {
        if (command.Email != "test@gmail.com" || command.Password != "123456")
        {
            return new ProblemDetails(
            [
                new ValidationFailure("email", "Authentication credentials is invalid")
                {
                    ErrorCode ="invalid_credentials"
                },
                new ValidationFailure("password", "Authentication credentials is invalid")
                {
                    ErrorCode ="invalid_credentials"
                }
            ], 400);
        }

        var generateRefreshTokenTask = Nanoid.GenerateAsync();
        var o = jwtOptions.Value;
        var now = DateTime.UtcNow;

        var certificate = X509Certificate2.CreateFromEncryptedPemFile(o.CertificateFilePath, o.KeyPassword, o.KeyFilePath);
        var signingCredentials = new SigningCredentials(new X509SecurityKey(certificate), SecurityAlgorithms.RsaSha256);
        var handler = new JwtSecurityTokenHandler();
        var accessTokenMaxAge = TimeSpan.FromMinutes(5);
        var accessToken = handler.CreateJwtSecurityToken(
            issuer: o.ValidIssuers.FirstOrDefault(),
            audience: o.ValidAudiences.FirstOrDefault(),
            subject: new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, command.Email),
            ]),
            notBefore: now,
            expires: now.Add(accessTokenMaxAge),
            issuedAt: now,
            signingCredentials: signingCredentials);

        return new AuthenticateResult(
            handler.WriteToken(accessToken),
            await generateRefreshTokenTask.ConfigureAwait(false),
            (int)accessTokenMaxAge.TotalSeconds,
            (int)TimeSpan.FromDays(1).TotalSeconds);
    }
}
